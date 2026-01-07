using AutoMapper;
using Microsoft.AspNetCore.Http;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.DAL.Repositories;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.DTOs.User;

namespace PeerDrop.BLL.Services;

public class UserService(IUserRepository userRepository, IMapper mapper, IFileService fileService) : IUserService
{

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        var users = await userRepository.GetAllAsync();
        return mapper.Map<IEnumerable<UserResponse>>(users);
    }

    public async Task<UserResponse> GetUserByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(ErrorMessages.UserNotFound, ErrorCodes.UserNotFound);

        return mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> GetUserByEmailAsync(string email)
    {
        var user = await userRepository.GetByEmailAsync(email)
            ?? throw new NotFoundException(ErrorMessages.UserNotFound, ErrorCodes.UserNotFound);

        return mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> UpdateUserAsync(Guid id, UserResponse userDto)
    {
        var user = await userRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(ErrorMessages.UserNotFound, ErrorCodes.UserNotFound);

        user.UserName = userDto.UserName;
        user.FullName = userDto.FullName;
        user.Avatar = userDto.Avatar;
        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await userRepository.UpdateAsync(user);
        return mapper.Map<UserResponse>(updatedUser);
    }

    public async Task<UserResponse> UploadAvatarAsync(Guid? id, IFormFile avatar)
    {
        var user = await userRepository.GetByIdAsync(id)
                   ?? throw new NotFoundException(ErrorMessages.UserNotFound, ErrorCodes.UserNotFound);
        await  ValidateFile(avatar);
        
        var avatarResposne = await fileService.UploadFileAsync(avatar);

        if (user.AvatarPublicId != null)
        {
            await fileService.DeleteFileByPublicIdAsync(user.AvatarPublicId);
        }
        
        user.Avatar = avatarResposne.SecureUrl;
        user.AvatarPublicId = avatarResposne.PublicId;
        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await userRepository.UpdateAsync(user);
        return mapper.Map<UserResponse>(updatedUser);
    }

    private static Task ValidateFile(IFormFile file)
    {
        if (file.Length > 5000000)
        {
            throw new FileTooLargeException(ErrorMessages.FileTooLarge, ErrorCodes.FileTooLarge);
        }
        
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        return !allowedExtensions.Contains(extension) ? throw new BusinessException(ErrorMessages.BadRequest, ErrorCodes.BadRequest) : Task.CompletedTask;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        if (!await userRepository.ExistsAsync(id))
        {
            throw new NotFoundException(ErrorMessages.UserNotFound, ErrorCodes.UserNotFound);
        }

        return await userRepository.DeleteAsync(id);
    }
}
