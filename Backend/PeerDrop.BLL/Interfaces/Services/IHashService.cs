namespace PeerDrop.BLL.Interfaces.Services;

public interface IHashService
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}