using Microsoft.AspNetCore.Http;
using PeerDrop.Shared.DTOs.Files;

namespace PeerDrop.BLL.Interfaces.Services;

public interface IFileStorageService
{
    Task<FileResponse> UploadAsync(Stream fileStream,
        string fileName,
        string? contentType = null,
        CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(string publicId,CancellationToken cancellationToken = default);
}