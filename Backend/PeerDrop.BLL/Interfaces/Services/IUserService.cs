using PeerDrop.Shared.DTOs;

namespace PeerDrop.BLL.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> GetUserByIdAsync(Guid id);
    Task<UserResponse> GetUserByEmailAsync(string email);
    Task<UserResponse> UpdateUserAsync(Guid id, UserResponse userDto);
    Task<bool> DeleteUserAsync(Guid id);
}
