namespace PeerDrop.Shared.DTOs.Auth;

public class RefreshTokenRequest
{
    public string RefreshToken {get; init;} = string.Empty;
    public string UserId { get; init; } = string.Empty;
}