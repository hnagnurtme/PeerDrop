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

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly IHashService _hashService;
    private readonly ICurrentUserService _currentUserService;

    public AuthService(
        IUserRepository userRepository, 
        IMapper mapper, 
        IOptions<JwtSettings> jwtSettings, 
        IHashService hashService,
        ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _jwtSettings = jwtSettings.Value;
        _hashService = hashService;
        _currentUserService = currentUserService;
    }
    

    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email)
            ?? throw new UnauthorizedException(ErrorMessages.InvalidCredentials, ErrorCodes.AuthInvalidCredentials);

        if (!_hashService.Verify(password, user.PasswordHash))
        {
            throw new UnauthorizedException(ErrorMessages.InvalidCredentials, ErrorCodes.AuthInvalidCredentials);
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException(ErrorMessages.AccountDisabled, ErrorCodes.AuthAccountDisabled);
        }

        return await GenerateAndSaveTokensAsync(user);
    }

    public async Task<AuthResponse> RegisterAsync(string email, string password, string fullName, string userName)
    {
        if (await _userRepository.EmailExistsAsync(email))
        {
            throw new UnprocessableEntityException(ErrorMessages.EmailAlreadyExists, ErrorCodes.AuthEmailAlreadyExists);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = userName,
            PasswordHash = _hashService.Hash(password),
            FullName = fullName,
            Role = UserRole.User,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);

        return await GenerateAndSaveTokensAsync(user);
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken, string userId)
    {
        if (!Guid.TryParse(userId, out var userGuid))
        {
            throw new UnauthorizedException(ErrorMessages.InvalidToken, ErrorCodes.AuthInvalidToken);
        }

        var user = await _userRepository.GetByIdAsync(userGuid)
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
        return await GenerateAndSaveTokensAsync(user);
    }

    public async Task LogoutAsync()
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            return;
        }

        var user = await _userRepository.GetByIdAsync(userId.Value);
        if (user != null)
        {
            // Invalidate refresh token
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
        }
    }

    private async Task<AuthResponse> GenerateAndSaveTokensAsync(User user)
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
        await _userRepository.UpdateAsync(user);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryInMinutes),
            User = new UserInfo
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                Avatar = user.Avatar
            }
        };
    }
}
