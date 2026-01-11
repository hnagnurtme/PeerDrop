using AutoMapper;
using Microsoft.AspNetCore.Http;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.DAL.Repositories;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.DTOs.User;

namespace PeerDrop.BLL.Services;

public class UserService(IUserRepository userRepository, IMapper mapper, IFileService fileService, ICurrentUserService currentUserService) : IUserService
{

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        return mapper.Map<IEnumerable<UserResponse>>(users);
    }

    public async Task<UserResponse> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(ErrorMessages.UserNotFound, ErrorCodes.UserNotFound);

        return mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(email, cancellationToken)
            ?? throw new NotFoundException(ErrorMessages.UserNotFound, ErrorCodes.UserNotFound);

        return mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest updateRequest, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(ErrorMessages.UserNotFound, ErrorCodes.UserNotFound);

        user.UserName = updateRequest.UserName;
        user.FullName = updateRequest.FullName;
        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await userRepository.UpdateAsync(user, cancellationToken);
        return mapper.Map<UserResponse>(updatedUser);
    }

    public async Task<UserResponse> UploadAvatarAsync(IFormFile avatar, CancellationToken cancellationToken = default)
    {
        var currentUserId = currentUserService.UserId!.Value;
        var user = await userRepository.GetByIdAsync(currentUserId, cancellationToken)
                   ?? throw new NotFoundException(ErrorMessages.UserNotFound, ErrorCodes.UserNotFound);
        await  ValidateFile(avatar);
        
        var avatarResponse = await fileService.UploadFileAsync(avatar, cancellationToken);

        if (user.AvatarPublicId != null)
        {
            await fileService.DeleteFileByPublicIdAsync(user.AvatarPublicId, cancellationToken);
        }
        
        user.Avatar = avatarResponse.SecureUrl;
        user.AvatarPublicId = avatarResponse.PublicId;
        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await userRepository.UpdateAsync(user, cancellationToken);
        return mapper.Map<UserResponse>(updatedUser);
    }

    private static Task ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new BusinessException(ErrorMessages.BadRequest, ErrorCodes.BadRequest);
        }
        
        if (file.Length > ProjectConstants.FileUpload.MaxAvatarSizeBytes)
        {
            throw new FileTooLargeException(ErrorMessages.FileTooLarge, ErrorCodes.FileTooLarge);
        }
        
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        return !ProjectConstants.FileUpload.AllowedAvatarExtensions.Contains(extension) 
            ? throw new BusinessException(ErrorMessages.BadRequest, ErrorCodes.BadRequest) 
            : Task.CompletedTask;
    }

    public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (!await userRepository.ExistsAsync(id, cancellationToken))
        {
            throw new NotFoundException(ErrorMessages.UserNotFound, ErrorCodes.UserNotFound);
        }

        return await userRepository.DeleteAsync(id, cancellationToken);
    }
}
