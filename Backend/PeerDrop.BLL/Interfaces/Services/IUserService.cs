using Microsoft.AspNetCore.Http;
using PeerDrop.Shared.DTOs.User;

namespace PeerDrop.BLL.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<UserResponse> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserResponse> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest updateRequest, CancellationToken cancellationToken = default);
    Task<UserResponse> UploadAvatarAsync(IFormFile avatar, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
}
