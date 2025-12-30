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
    public AuthService(IUserRepository userRepository, IMapper mapper, IOptions<JwtSettings> jwtSettings, IHashService hashService)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _jwtSettings = jwtSettings.Value;
        _mapper = mapper;
        _hashService = hashService;
    }
    

    public async Task<LoginResponse> LoginAsync(string email, string password)
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

        return GenerateLoginResponse(user);
    }

    public async Task<LoginResponse> RegisterAsync(string email, string password, string fullName)
    {
        if (await _userRepository.EmailExistsAsync(email))
        {
            throw new UnprocessableEntityException(ErrorMessages.EmailAlreadyExists, ErrorCodes.AuthEmailAlreadyExists);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = _hashService.Hash(password),
            FullName = fullName,
            Role = UserRole.User,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);

        return GenerateLoginResponse(user);
    }

    public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
    {
        // In a real application, you would validate the refresh token
        // and retrieve the user associated with it
        throw new NotImplementedException("Refresh token logic needs to be implemented");
    }

    public Task LogoutAsync()
    {
        // In a real application, you would invalidate the refresh token
        return Task.CompletedTask;
    }

    private LoginResponse GenerateLoginResponse(User user)
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

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryInMinutes),
            User = new UserInfo
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString()
            }
        };
    }
}
