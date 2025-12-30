using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.BLL.Services;
using PeerDrop.DAL.Entities;
using PeerDrop.DAL.Repositories;
using PeerDrop.Shared.Configurations;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.Enums;
using Xunit;

namespace PeerDrop.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IHashService> _hashServiceMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IOptions<JwtSettings> _jwtSettings;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _hashServiceMock = new Mock<IHashService>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _mapperMock = new Mock<IMapper>();
        
        _jwtSettings = Options.Create(new JwtSettings
        {
            SecretKey = "ThisIsAVerySecureSecretKeyForJWTTokenGeneration123",
            Issuer = "PeerDrop",
            Audience = "PeerDrop",
            ExpiryInMinutes = 60
        });

        _authService = new AuthService(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _jwtSettings,
            _hashServiceMock.Object,
            _currentUserServiceMock.Object
        );
    }

    #region LoginAsync Tests

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnAuthResponse()
    {
        // Arrange
        var email = "test@example.com";
        var password = "Password123";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = "hashedPassword",
            FullName = "Test User",
            Role = UserRole.User,
            IsActive = true
        };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(user);
        _hashServiceMock.Setup(x => x.Verify(password, user.PasswordHash))
            .Returns(true);
        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.LoginAsync(email, password);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.User.Email.Should().Be(email);
        result.User.FullName.Should().Be(user.FullName);
        
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidEmail_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var password = "Password123";

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var act = async () => await _authService.LoginAsync(email, password);
        
        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage(ErrorMessages.InvalidCredentials);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "WrongPassword";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = "hashedPassword",
            IsActive = true
        };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(user);
        _hashServiceMock.Setup(x => x.Verify(password, user.PasswordHash))
            .Returns(false);

        // Act & Assert
        var act = async () => await _authService.LoginAsync(email, password);
        
        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage(ErrorMessages.InvalidCredentials);
    }

    [Fact]
    public async Task LoginAsync_WithInactiveAccount_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "Password123";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = "hashedPassword",
            IsActive = false
        };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(user);
        _hashServiceMock.Setup(x => x.Verify(password, user.PasswordHash))
            .Returns(true);

        // Act & Assert
        var act = async () => await _authService.LoginAsync(email, password);
        
        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage(ErrorMessages.AccountDisabled);
    }

    #endregion

    #region RegisterAsync Tests

    [Fact]
    public async Task RegisterAsync_WithValidData_ShouldReturnAuthResponse()
    {
        // Arrange
        var email = "newuser@example.com";
        var password = "Password123";
        var fullName = "New User";

        _userRepositoryMock.Setup(x => x.EmailExistsAsync(email))
            .ReturnsAsync(false);
        _hashServiceMock.Setup(x => x.Hash(password))
            .Returns("hashedPassword");
        _userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);
        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        // Act
        var result = await _authService.RegisterAsync(email, password, fullName);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.User.Email.Should().Be(email);
        result.User.FullName.Should().Be(fullName);
        
        _userRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ShouldThrowUnprocessableEntityException()
    {
        // Arrange
        var email = "existing@example.com";
        var password = "Password123";
        var fullName = "Test User";

        _userRepositoryMock.Setup(x => x.EmailExistsAsync(email))
            .ReturnsAsync(true);

        // Act & Assert
        var act = async () => await _authService.RegisterAsync(email, password, fullName);
        
        await act.Should().ThrowAsync<UnprocessableEntityException>()
            .WithMessage(ErrorMessages.EmailAlreadyExists);
    }

    #endregion

    #region RefreshTokenAsync Tests

    [Fact]
    public async Task RefreshTokenAsync_WithValidToken_ShouldReturnNewAuthResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshToken = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = userId,
            Email = "test@example.com",
            FullName = "Test User",
            Role = UserRole.User,
            IsActive = true,
            RefreshToken = refreshToken,
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
        };

        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.RefreshTokenAsync(refreshToken, userId.ToString());

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBe(refreshToken); // Should be new token
        
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RefreshTokenAsync_WithInvalidToken_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshToken = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = userId,
            RefreshToken = "different-token",
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
        };

        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        // Act & Assert
        var act = async () => await _authService.RefreshTokenAsync(refreshToken, userId.ToString());
        
        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage(ErrorMessages.InvalidToken);
    }

    [Fact]
    public async Task RefreshTokenAsync_WithExpiredToken_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshToken = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = userId,
            RefreshToken = refreshToken,
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(-1) // Expired
        };

        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        // Act & Assert
        var act = async () => await _authService.RefreshTokenAsync(refreshToken, userId.ToString());
        
        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage(ErrorMessages.TokenExpired);
    }

    [Fact]
    public async Task RefreshTokenAsync_WithInvalidUserId_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var refreshToken = Guid.NewGuid().ToString();
        var invalidUserId = "not-a-guid";

        // Act & Assert
        var act = async () => await _authService.RefreshTokenAsync(refreshToken, invalidUserId);
        
        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage(ErrorMessages.InvalidToken);
    }

    #endregion

    #region LogoutAsync Tests

    [Fact]
    public async Task LogoutAsync_WithAuthenticatedUser_ShouldInvalidateRefreshToken()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "test@example.com",
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
        };

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(userId);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        // Act
        await _authService.LogoutAsync();

        // Assert
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.Is<User>(u => 
            u.RefreshToken == null && 
            u.RefreshTokenExpiry == null
        )), Times.Once);
    }

    [Fact]
    public async Task LogoutAsync_WithoutAuthenticatedUser_ShouldNotThrow()
    {
        // Arrange
        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns((Guid?)null);

        // Act
        var act = async () => await _authService.LogoutAsync();

        // Assert
        await act.Should().NotThrowAsync();
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    #endregion
}
