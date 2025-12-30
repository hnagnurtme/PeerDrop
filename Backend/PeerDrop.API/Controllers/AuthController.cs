using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.Shared.DTOs.Auth;
using PeerDrop.Shared.Responses;

namespace PeerDrop.API.Controllers;

public class AuthController(IAuthService authService) : BaseApiController
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        var result = await authService.LoginAsync(request.Email, request.Password);
        return OkResponse(result, "Login successful");
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Register([FromBody] RegisterRequest request)
    {
        var result = await authService.RegisterAsync(request.Email, request.Password, request.FullName);
        return CreatedResponse(result, "Registration successful");
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> RefreshToken([FromBody] string refreshToken)
    {
        var result = await authService.RefreshTokenAsync(refreshToken);
        return OkResponse(result, "Token refreshed successfully");
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await authService.LogoutAsync();
        return NoContentResponse();
    }
}
