using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.DTOs.Files;

namespace PeerDrop.BLL.Services;

public class FileService(Cloudinary cloudinary) : IFileService
{

    public async Task<FileResponse> UploadFileAsync(IFormFile file)
    {
        await  ValidateFile(file);

        await using var stream = file.OpenReadStream();

        var uploadParams = new AutoUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "peerdrop",
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false,
        };
        
        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception(uploadResult.Error.ToString());
        }

        return new FileResponse
        {
            PublicId = uploadResult.PublicId,
            SecureUrl  = uploadResult.SecureUrl?.ToString() ?? string.Empty
        };
    }
    
    public async Task<bool> DeleteFileByPublicIdAsync(string publicId)
    {
        if(string.IsNullOrWhiteSpace(publicId))
        {
            return false;
        }
        var deleteParams = new DeletionParams(publicId)
        {
            ResourceType = ResourceType.Auto
        };

        var result = await  cloudinary.DestroyAsync(deleteParams);
        return result.Result is "ok" or "not found";
    }

    private static Task ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new BusinessException(ErrorMessages.CloudStorageForbidden, ErrorCodes.CloudStorageForbidden);
        }
        return file.Length > 1024 * 1024 * 10 ? throw new FileTooLargeException(ErrorMessages.FileTooLarge, ErrorCodes.FileTooLarge) : Task.CompletedTask;
    }
}