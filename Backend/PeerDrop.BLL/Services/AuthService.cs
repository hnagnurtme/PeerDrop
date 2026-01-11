using AutoMapper;
using Microsoft.Extensions.Options;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.DAL.Entities;
using PeerDrop.DAL.Repositories;
using PeerDrop.Shared.Configurations;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.DTOs.Auth;
using PeerDrop.Shared.Enums;
using PeerDrop.Shared.Helpers;

namespace PeerDrop.BLL.Services;

public class AuthService(
    IUserRepository userRepository,
    IOptions<JwtSettings> jwtSettings,
    IHashService hashService,
    ICurrentUserService currentUserService
)
    : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;


    public async Task<AuthResponse> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(email, cancellationToken)
            ?? throw new UnauthorizedException(ErrorMessages.InvalidCredentials, ErrorCodes.AuthInvalidCredentials);

        if (!hashService.Verify(password, user.PasswordHash))
        {
            throw new UnauthorizedException(ErrorMessages.InvalidCredentials, ErrorCodes.AuthInvalidCredentials);
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException(ErrorMessages.AccountDisabled, ErrorCodes.AuthAccountDisabled);
        }

        return await GenerateAndSaveTokensAsync(user, cancellationToken);
    }

    public async Task<AuthResponse> RegisterAsync(string email, string password, string fullName, string userName, CancellationToken cancellationToken = default)
    {
        if (await userRepository.EmailExistsAsync(email, cancellationToken))
        {
            throw new UnprocessableEntityException(ErrorMessages.EmailAlreadyExists, ErrorCodes.AuthEmailAlreadyExists);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = userName,
            PasswordHash = hashService.Hash(password),
            FullName = fullName,
            Role = UserRole.User,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.CreateAsync(user, cancellationToken);

        return await GenerateAndSaveTokensAsync(user, cancellationToken);
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken, string userId, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(userId, out var userGuid))
        {
            throw new UnauthorizedException(ErrorMessages.InvalidToken, ErrorCodes.AuthInvalidToken);
        }

        var user = await userRepository.GetByIdAsync(userGuid, cancellationToken)
            ?? throw new UnauthorizedException(ErrorMessages.InvalidToken, ErrorCodes.AuthInvalidToken);

        // Validate refresh token
        if (user.RefreshToken != refreshToken)
        {
            throw new UnauthorizedException(ErrorMessages.InvalidToken, ErrorCodes.AuthInvalidToken);
        }

        // Check if refresh token has expired
        if (user.RefreshTokenExpiry == null || user.RefreshTokenExpiry <= DateTime.UtcNow)
        {
            throw new UnauthorizedException(ErrorMessages.TokenExpired, ErrorCodes.AuthTokenExpired);
        }

        // Generate new tokens
        return await GenerateAndSaveTokensAsync(user, cancellationToken);
    }

    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        var userId = currentUserService.UserId;
        if (userId == null)
        {
            return;
        }

        var user = await userRepository.GetByIdAsync(userId.Value, cancellationToken);
        if (user != null)
        {
            // Invalidate refresh token
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;
            await userRepository.UpdateAsync(user, cancellationToken);
        }
    }

    async private Task<AuthResponse> GenerateAndSaveTokensAsync(User user, CancellationToken cancellationToken = default)
    {
        var secretKey = _jwtSettings.SecretKey;
        var issuer = _jwtSettings.Issuer;
        var audience = _jwtSettings.Audience;
        var expiryInMinutes = _jwtSettings.ExpiryInMinutes;

        var accessToken = JwtHelper.GenerateToken(
            user.Id,
            user.Email,
            user.Role.ToString(),
            secretKey,
            issuer,
            audience,
            expiryInMinutes);

        // Generate refresh token and set expiry (7 days)
        var refreshToken = Guid.NewGuid().ToString();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        // Update user with new refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = refreshTokenExpiry;
        user.UpdatedAt = DateTime.UtcNow;
        await userRepository.UpdateAsync(user, cancellationToken);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = new UserInfo
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName,
                Avatar = user.Avatar
            }
        };
    }
}
