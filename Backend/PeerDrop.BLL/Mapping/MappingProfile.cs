using AutoMapper;
using PeerDrop.BLL.Models;
using PeerDrop.DAL.Entities;
using PeerDrop.Shared.DTOs.User;

namespace PeerDrop.BLL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity to DTO
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        // Entity to Model
        CreateMap<User, UserModel>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        // Model to DTO
        CreateMap<UserModel, UserResponse>();

        // DTO to Model (for updates)
        CreateMap<UserResponse, UserModel>();
    }
}
