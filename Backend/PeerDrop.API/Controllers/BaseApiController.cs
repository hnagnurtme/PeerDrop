using Microsoft.AspNetCore.Mvc;
using PeerDrop.Shared.Responses;

namespace PeerDrop.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<ApiResponse<T>> OkResponse<T>(T data, string message = "Success")
    {
        return Ok(ApiResponse<T>.Success(data, message));
    }
    
    protected ActionResult<ApiResponse<T>> CreatedResponse<T>(T data, string message = "Created successfully")
    {
        return StatusCode(201, ApiResponse<T>.Success(data, message));
    }
    
    protected ActionResult NoContentResponse()
    {
        return NoContent();
    }
    
    protected ActionResult<ApiResponse<T>> BadRequestResponse<T>(string message, object? errors = null, string? errorCode = null)
    {
        return BadRequest(ApiResponse<T>.Fail(message, errors, errorCode));
    }
    
    protected ActionResult<ApiResponse<T>> NotFoundResponse<T>(string message, string? errorCode = null)
    {
        return NotFound(ApiResponse<T>.Fail(message, errorCode: errorCode));
    }
}
