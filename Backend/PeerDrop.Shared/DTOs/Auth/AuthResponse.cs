namespace PeerDrop.Shared.DTOs.Auth;

public class AuthResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public UserInfo User { get; init; } = new UserInfo();
}

public class UserInfo
{
    public Guid Id { get; set; }
    public string Email { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? Avatar { get; set; }
}
