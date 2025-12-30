using PeerDrop.Shared.DTOs.Auth;

namespace PeerDrop.BLL.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(string email, string password);
    Task<AuthResponse> RegisterAsync(string email, string password, string fullName);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken, string userId);
    Task LogoutAsync();
}
