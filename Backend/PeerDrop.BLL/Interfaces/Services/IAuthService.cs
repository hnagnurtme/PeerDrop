using PeerDrop.Shared.DTOs.Auth;

namespace PeerDrop.BLL.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(string email, string password);
    Task<LoginResponse> RegisterAsync(string email, string password, string fullName);
    Task<LoginResponse> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync();
}
