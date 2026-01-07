using Microsoft.AspNetCore.Http;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.DTOs.Files;

namespace PeerDrop.BLL.Services;

public class FileService(IFileStorageService fileStorageService) : IFileService
{

    public async Task<FileResponse> UploadFileAsync(IFormFile file,CancellationToken cancellationToken = default)
    {
        ValidateFile(file);

        await using var stream = file.OpenReadStream();
        
        return await fileStorageService.UploadAsync(stream, file.FileName, file.ContentType, cancellationToken);
    }
    
    public async Task<bool> DeleteFileByPublicIdAsync(string publicId,CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrWhiteSpace(publicId))
        {
            return false;
        }
        return await fileStorageService.DeleteAsync(publicId, cancellationToken);
    }

    private static void ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new BusinessException(ErrorMessages.InvalidFile, ErrorCodes.InvalidFile);
        }
        if (file.Length > ProjectConstants.FileUpload.MaxFileSizeBytes)
        {
            throw new FileTooLargeException(ErrorMessages.FileTooLarge, ErrorCodes.FileTooLarge);
        }
    }
}