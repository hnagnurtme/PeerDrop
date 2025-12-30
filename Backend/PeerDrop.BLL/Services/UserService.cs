using AutoMapper;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.DAL.Entities;
using PeerDrop.DAL.Repositories;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.DTOs;

namespace PeerDrop.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponse>>(users);
    }

    public async Task<UserResponse> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(ErrorMessages.UserNotFound);

        return _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email)
            ?? throw new NotFoundException(ErrorMessages.UserNotFound);

        return _mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> UpdateUserAsync(Guid id, UserResponse userDto)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(ErrorMessages.UserNotFound);

        user.FullName = userDto.FullName;
        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserResponse>(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        if (!await _userRepository.ExistsAsync(id))
        {
            throw new NotFoundException(ErrorMessages.UserNotFound);
        }

        return await _userRepository.DeleteAsync(id);
    }
}
