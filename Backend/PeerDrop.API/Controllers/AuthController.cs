using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeerDrop.API.Attributes;
using PeerDrop.API.Constants;
using PeerDrop.API.Documentations;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.Shared.DTOs.Auth;
using PeerDrop.Shared.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace PeerDrop.API.Controllers;

public class AuthController(IAuthService authService) : BaseApiController
{

    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = AuthEndpoints.Login.Summary, Description = AuthEndpoints.Login.Description)]
    [StandardResponseTypes(typeof(AuthResponse))]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request.Email, request.Password, cancellationToken);
        return OkResponse(result, ApiMessages.Auth.LoginSuccessful);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = AuthEndpoints.Register.Summary, Description = AuthEndpoints.Register.Description)]
    [StandardResponseTypes(typeof(AuthResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request.Email, request.Password, request.FullName, request.UserName, cancellationToken);
        return CreatedResponse(result, ApiMessages.Auth.RegistrationSuccessful);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = AuthEndpoints.RefreshToken.Summary, Description = AuthEndpoints.RefreshToken.Description)]
    [StandardResponseTypes(typeof(AuthResponse))]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RefreshTokenAsync(request.RefreshToken, request.UserId, cancellationToken);
        return OkResponse(result, ApiMessages.Auth.TokenRefreshed);
    }

    [HttpPost("logout")]
    [Authorize]
    [SwaggerOperation(Summary = AuthEndpoints.Logout.Summary, Description = AuthEndpoints.Logout.Description)]
    public async Task<ActionResult> Logout(CancellationToken cancellationToken)
    {
        await authService.LogoutAsync(cancellationToken);
        return NoContentResponse();
    }
}
