using Microsoft.AspNetCore.Http;
using PeerDrop.Shared.DTOs.User;

namespace PeerDrop.BLL.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> GetUserByIdAsync(Guid id);
    Task<UserResponse> GetUserByEmailAsync(string email);
    Task<UserResponse> UpdateUserAsync(Guid id, UserResponse userDto);
    Task<UserResponse> UploadAvatarAsync(Guid? id, IFormFile avatar);
    Task<bool> DeleteUserAsync(Guid id);
}
