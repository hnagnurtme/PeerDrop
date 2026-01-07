using Microsoft.AspNetCore.Http;
using PeerDrop.Shared.DTOs.Files;

namespace PeerDrop.BLL.Interfaces.Services;

public interface IFileService
{
    Task<FileResponse> UploadFileAsync(IFormFile file);

    Task<bool> DeleteFileByPublicIdAsync(String publicId);
}