using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.Shared.DTOs.User;
using PeerDrop.Shared.Responses;

namespace PeerDrop.API.Controllers;

[Authorize]
public class UsersController(IUserService userService, ICurrentUserService currentUserService) : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserResponse>>>> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return OkResponse(users, "Users retrieved successfully");
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserById(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        return OkResponse(user, "User retrieved successfully");
    }

    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserByEmail(string email)
    {
        var user = await userService.GetUserByEmailAsync(email);
        return OkResponse(user, "User retrieved successfully");
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<UserResponse>>> UpdateUser(Guid id, [FromBody] UserResponse userDto)
    {
        var user = await userService.UpdateUserAsync(id, userDto);
        return OkResponse(user, "User updated successfully");
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        await userService.DeleteUserAsync(id);
        return NoContentResponse();
    }

    [HttpPost("avatar")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<UserResponse>>> UploadAvatar([FromForm] IFormFile avatar)
    {
        var currentUserId = currentUserService.UserId;
        if (currentUserId == null)
        {
            return Unauthorized("User not authenticated");
        }

        var user = await userService.UploadAvatarAsync(currentUserId, avatar);
        return OkResponse(user, "Avatar uploaded successfully");
    }
}
