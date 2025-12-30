using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.Shared.DTOs;
using PeerDrop.Shared.Responses;

namespace PeerDrop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserResponse>>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(ApiResponse<IEnumerable<UserResponse>>.Success(users, "Users retrieved successfully"));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(ApiResponse<UserResponse>.Success(user, "User retrieved successfully"));
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<ApiResponse<UserResponse>>> GetUserByEmail(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        return Ok(ApiResponse<UserResponse>.Success(user, "User retrieved successfully"));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<UserResponse>>> UpdateUser(Guid id, [FromBody] UserResponse userDto)
    {
        var user = await _userService.UpdateUserAsync(id, userDto);
        return Ok(ApiResponse<UserResponse>.Success(user, "User updated successfully"));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        return Ok(ApiResponse<bool>.Success(result, "User deleted successfully"));
    }
}
