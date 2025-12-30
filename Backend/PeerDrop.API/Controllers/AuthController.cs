using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.Shared.DTOs.Auth;
using PeerDrop.Shared.Responses;

namespace PeerDrop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        var result = await authService.LoginAsync(request.Email, request.Password);
        return Ok(ApiResponse<LoginResponse>.Success(result, "Login successful"));
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Register([FromBody] RegisterRequest request)
    {
        var result = await authService.RegisterAsync(request.Email, request.Password, request.FullName);
        return Ok(ApiResponse<LoginResponse>.Success(result, "Registration successful"));
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> RefreshToken([FromBody] string refreshToken)
    {
        var result = await authService.RefreshTokenAsync(refreshToken);
        return Ok(ApiResponse<LoginResponse>.Success(result, "Token refreshed successfully"));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<bool>>> Logout()
    {
        await authService.LogoutAsync();
        return Ok(ApiResponse<bool>.Success(true, "Logged out successfully"));
    }
}
