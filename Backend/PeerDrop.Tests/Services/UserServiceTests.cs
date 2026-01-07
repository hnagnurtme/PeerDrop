using FluentAssertions;
using AutoMapper;
using Moq;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.BLL.Services;
using PeerDrop.DAL.Entities;
using PeerDrop.DAL.Repositories;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.DTOs.User;
using PeerDrop.Shared.Enums;
using Xunit;

namespace PeerDrop.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _fileServiceMock = new Mock<IFileService>();
        _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object, _fileServiceMock.Object);
    }

    #region GetAllUsersAsync Tests

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = Guid.NewGuid(), Email = "user1@example.com", FullName = "User 1", Role = UserRole.User },
            new() { Id = Guid.NewGuid(), Email = "user2@example.com", FullName = "User 2", Role = UserRole.Admin }
        };

        var userResponses = new List<UserResponse>
        {
            new() { Id = users[0].Id, Email = "user1@example.com", FullName = "User 1", Role = "User" },
            new() { Id = users[1].Id, Email = "user2@example.com", FullName = "User 2", Role = "Admin" }
        };

        _userRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);
        
        _mapperMock.Setup(x => x.Map<IEnumerable<UserResponse>>(users))
            .Returns(userResponses);

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(u => u.Email == "user1@example.com");
        result.Should().Contain(u => u.Email == "user2@example.com");
    }

    #endregion

    #region GetUserByIdAsync Tests

    [Fact]
    public async Task GetUserByIdAsync_WithExistingId_ShouldReturnUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "test@example.com",
            FullName = "Test User",
            Role = UserRole.User
        };

        var userResponse = new UserResponse
        {
            Id = userId,
            Email = "test@example.com",
            FullName = "Test User",
            Role = "User"
        };

        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);
        
        _mapperMock.Setup(x => x.Map<UserResponse>(user))
            .Returns(userResponse);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(userId);
        result.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task GetUserByIdAsync_WithNonExistingId_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var act = async () => await _userService.GetUserByIdAsync(userId);
        
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage(ErrorMessages.UserNotFound);
    }

    #endregion

    #region GetUserByEmailAsync Tests

    [Fact]
    public async Task GetUserByEmailAsync_WithExistingEmail_ShouldReturnUser()
    {
        // Arrange
        var email = "test@example.com";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FullName = "Test User",
            Role = UserRole.User
        };

        var userResponse = new UserResponse
        {
            Id = user.Id,
            Email = email,
            FullName = "Test User",
            Role = "User"
        };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(user);
        
        _mapperMock.Setup(x => x.Map<UserResponse>(user))
            .Returns(userResponse);

        // Act
        var result = await _userService.GetUserByEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(email);
    }

    [Fact]
    public async Task GetUserByEmailAsync_WithNonExistingEmail_ShouldThrowNotFoundException()
    {
        // Arrange
        var email = "nonexistent@example.com";
        
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var act = async () => await _userService.GetUserByEmailAsync(email);
        
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage(ErrorMessages.UserNotFound);
    }

    #endregion

    #region UpdateUserAsync Tests

    [Fact]
    public async Task UpdateUserAsync_WithExistingUser_ShouldReturnUpdatedUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new User
        {
            Id = userId,
            Email = "old@example.com",
            FullName = "Old Name",
            Role = UserRole.User
        };

        var updatedUser = new User
        {
            Id = userId,
            Email = "new@example.com",
            FullName = "New Name",
            Role = UserRole.User
        };
        
        var updatedData = new UserResponse
        {
            Id = userId,
            Email = "new@example.com",
            FullName = "New Name",
            Role = "User"
        };

        var updatedResponse = new UserResponse
        {
            Id = userId,
            Email = "new@example.com",
            FullName = "New Name",
            Role = "User"
        };

        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(updatedUser);
        _mapperMock.Setup(x => x.Map<UserResponse>(It.IsAny<User>()))
            .Returns(updatedResponse);

        // Act
        var result = await _userService.UpdateUserAsync(userId, updatedData);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("new@example.com");
        result.FullName.Should().Be("New Name");
        
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_WithNonExistingUser_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updatedData = new UserResponse
        {
            Id = userId,
            Email = "new@example.com",
            FullName = "New Name",
            Role = "User"
        };

        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var act = async () => await _userService.UpdateUserAsync(userId, updatedData);
        
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage(ErrorMessages.UserNotFound);
    }

    #endregion

    #region DeleteUserAsync Tests

    [Fact]
    public async Task DeleteUserAsync_WithExistingUser_ShouldReturnTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepositoryMock.Setup(x => x.ExistsAsync(userId))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(x => x.DeleteAsync(userId))
            .ReturnsAsync(true);

        // Act
        var result = await _userService.DeleteUserAsync(userId);

        // Assert
        result.Should().BeTrue();
        _userRepositoryMock.Verify(x => x.DeleteAsync(userId), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_WithNonExistingUser_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepositoryMock.Setup(x => x.ExistsAsync(userId))
            .ReturnsAsync(false);

        // Act & Assert
        var act = async () => await _userService.DeleteUserAsync(userId);
        
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage(ErrorMessages.UserNotFound);
    }

    #endregion
}
