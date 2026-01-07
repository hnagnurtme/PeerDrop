using Microsoft.AspNetCore.Http;
using PeerDrop.Shared.DTOs.Files;

namespace PeerDrop.BLL.Interfaces.Services;

public interface IFileService
{
    Task<FileResponse> UploadFileAsync(IFormFile file,CancellationToken cancellationToken = default);

    Task<bool> DeleteFileByPublicIdAsync(String publicId,CancellationToken cancellationToken = default);
}