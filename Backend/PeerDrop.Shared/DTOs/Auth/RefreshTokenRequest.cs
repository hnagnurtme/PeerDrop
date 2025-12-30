namespace PeerDrop.Shared.DTOs.Auth;

public class RefreshTokenRequest
{
    public string RefreshToken {get; set;} = string.Empty;
    public string UserId { get; set; } = string.Empty;
}