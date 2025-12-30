using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.Shared.DTOs;
using PeerDrop.Shared.Responses;

namespace PeerDrop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(IUserService userService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserResponse>>>> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(ApiResponse<IEnumerable<UserResponse>>.Success(users, "Users retrieved successfully"));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserById(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        return Ok(ApiResponse<UserResponse>.Success(user, "User retrieved successfully"));
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserByEmail(string email)
    {
        var user = await userService.GetUserByEmailAsync(email);
        return Ok(ApiResponse<UserResponse>.Success(user, "User retrieved successfully"));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<UserResponse>>> UpdateUser(Guid id, [FromBody] UserResponse userDto)
    {
        var user = await userService.UpdateUserAsync(id, userDto);
        return Ok(ApiResponse<UserResponse>.Success(user, "User updated successfully"));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(Guid id)
    {
        var result = await userService.DeleteUserAsync(id);
        return Ok(ApiResponse<bool>.Success(result, "User deleted successfully"));
    }
}
