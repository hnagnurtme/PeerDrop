using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeerDrop.API.Attributes;
using PeerDrop.API.Constants;
using PeerDrop.API.Documentations;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.Shared.DTOs.User;
using PeerDrop.Shared.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace PeerDrop.API.Controllers;

[Authorize]
public class UsersController(IUserService userService)
    : BaseApiController
{

    [HttpGet]
    [SwaggerOperation(Summary = UserEndpoints.GetAllUsers.Summary, Description = UserEndpoints.GetAllUsers.Description)]
    [StandardResponseTypes(typeof(IEnumerable<UserResponse>))]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserResponse>>>> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await userService.GetAllUsersAsync(cancellationToken);
        return OkResponse(users, ApiMessages.Users.UsersRetrieved);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = UserEndpoints.GetUserById.Summary, Description = UserEndpoints.GetUserById.Description)]
    [StandardResponseTypes(typeof(UserResponse))]
    public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(id, cancellationToken);
        return OkResponse(user, ApiMessages.Users.UserRetrieved);
    }

    [HttpGet("email/{email}")]
    [SwaggerOperation(Summary = UserEndpoints.GetUserByEmail.Summary, Description = UserEndpoints.GetUserByEmail.Description)]
    [StandardResponseTypes(typeof(UserResponse))]
    public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByEmailAsync(email, cancellationToken);
        return OkResponse(user, ApiMessages.Users.UserRetrieved);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = UserEndpoints.UpdateUser.Summary, Description = UserEndpoints.UpdateUser.Description)]
    [StandardResponseTypes(typeof(UserResponse))]
    public async Task<ActionResult<ApiResponse<UserResponse>>> UpdateUser(Guid id, [FromBody] UpdateUserRequest updateRequest, CancellationToken cancellationToken)
    {
        var user = await userService.UpdateUserAsync(id, updateRequest, cancellationToken);
        return OkResponse(user, ApiMessages.Users.UserUpdated);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = UserEndpoints.DeleteUser.Summary, Description = UserEndpoints.DeleteUser.Description)]
    [StandardResponseTypes(typeof(object), StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        await userService.DeleteUserAsync(id, cancellationToken);
        return NoContentResponse();
    }

    [HttpPost("avatar")]
    [Consumes("multipart/form-data")]
    [SwaggerOperation(Summary = UserEndpoints.UploadAvatar.Summary, Description = UserEndpoints.UploadAvatar.Description)]
    [StandardResponseTypes(typeof(UserResponse))]
    public async Task<ActionResult<ApiResponse<UserResponse>>> UploadAvatar([FromForm] IFormFile avatar, CancellationToken cancellationToken)
    {
        var user = await userService.UploadAvatarAsync(avatar, cancellationToken);
        return OkResponse(user, ApiMessages.Users.AvatarUploaded);
    }
}
