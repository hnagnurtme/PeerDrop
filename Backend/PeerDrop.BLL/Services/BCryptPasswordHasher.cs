using PeerDrop.BLL.Interfaces.Services;

namespace PeerDrop.BLL.Services;

public class BCryptPasswordHasher : IHashService
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool Verify(string password, string passwordHash)
    { 
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}