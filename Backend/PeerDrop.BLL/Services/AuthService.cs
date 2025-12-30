using AutoMapper;
using Microsoft.Extensions.Configuration;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.DAL.Entities;
using PeerDrop.DAL.Repositories;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.DTOs.Auth;
using PeerDrop.Shared.Enums;
using PeerDrop.Shared.Helpers;

namespace PeerDrop.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email)
            ?? throw new NotFoundException(ErrorMessages.UserNotFound);

        if (!VerifyPassword(password, user.PasswordHash))
        {
            throw new BusinessException(ErrorMessages.InvalidCredentials);
        }

        if (!user.IsActive)
        {
            throw new BusinessException(ErrorMessages.AccountDisabled);
        }

        return GenerateLoginResponse(user);
    }

    public async Task<LoginResponse> RegisterAsync(string email, string password, string fullName)
    {
        if (await _userRepository.EmailExistsAsync(email))
        {
            throw new BusinessException(ErrorMessages.EmailAlreadyExists);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = HashPassword(password),
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
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"]!;
        var issuer = jwtSettings["Issuer"]!;
        var audience = jwtSettings["Audience"]!;
        var expiryInMinutes = int.Parse(jwtSettings["ExpiryInMinutes"] ?? "60");

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

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
