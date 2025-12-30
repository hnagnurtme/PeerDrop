using AutoMapper;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.DAL.Entities;
using PeerDrop.DAL.Repositories;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.DTOs;

namespace PeerDrop.BLL.Services;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
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

        user.FullName = userDto.FullName;
        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await userRepository.UpdateAsync(user);
        return mapper.Map<UserResponse>(updatedUser);
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
