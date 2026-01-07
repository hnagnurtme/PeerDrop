using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.DTOs.Files;

namespace PeerDrop.BLL.ExternalServices;

public class CloudinaryFileStorageService(Cloudinary cloudinary) : IFileStorageService
{
    public async Task<FileResponse> UploadAsync(Stream fileStream, string fileName, string? contentType = null,CancellationToken cancellationToken = default)
    {
        var uploadParams = new AutoUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = ProjectConstants.FileUpload.DefaultFolder,
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false,
        };
        
        var uploadResult = await cloudinary.UploadAsync(uploadParams);
        if (uploadResult.StatusCode != HttpStatusCode.OK)
        {
            throw new BusinessException(
                uploadResult.Error?.Message ?? ErrorMessages.CloudUploadFailed,
                ErrorCodes.CloudUploadFailed
            );
        }

        return new FileResponse
        {
            PublicId = uploadResult.PublicId,
            SecureUrl = uploadResult.SecureUrl?.ToString() ?? string.Empty
        };
    }
    public async Task<bool> DeleteAsync(string publicId,CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(publicId))
            return false;

        var result = await cloudinary.DestroyAsync(
            new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Auto
            });

        return result.Result is
            ProjectConstants.Cloudinary.ResultOk or
            ProjectConstants.Cloudinary.ResultNotFound;
    }
}