using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using PeerDrop.BLL.Services;
using Xunit;

namespace PeerDrop.Tests.Services;

public class CurrentUserServiceTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly CurrentUserService _currentUserService;

    public CurrentUserServiceTests()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _currentUserService = new CurrentUserService(_httpContextAccessorMock.Object);
    }

    [Fact]
    public void UserId_WithValidClaim_ShouldReturnGuid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        
        _httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(httpContext);

        // Act
        var result = _currentUserService.UserId;

        // Assert
        result.Should().Be(userId);
    }

    [Fact]
    public void UserId_WithoutClaim_ShouldReturnNull()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(httpContext);

        // Act
        var result = _currentUserService.UserId;

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Email_WithValidClaim_ShouldReturnEmail()
    {
        // Arrange
        var email = "test@example.com";
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, email)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        
        _httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(httpContext);

        // Act
        var result = _currentUserService.Email;

        // Assert
        result.Should().Be(email);
    }

    [Fact]
    public void Role_WithValidClaim_ShouldReturnRole()
    {
        // Arrange
        var role = "Admin";
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, role)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        
        _httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(httpContext);

        // Act
        var result = _currentUserService.Role;

        // Assert
        result.Should().Be(role);
    }

    [Fact]
    public void IsAuthenticated_WithAuthenticatedUser_ShouldReturnTrue()
    {
        // Arrange
        var identity = new ClaimsIdentity("TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        
        _httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(httpContext);

        // Act
        var result = _currentUserService.IsAuthenticated;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsAuthenticated_WithoutAuthenticatedUser_ShouldReturnFalse()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(httpContext);

        // Act
        var result = _currentUserService.IsAuthenticated;

        // Assert
        result.Should().BeFalse();
    }
}
