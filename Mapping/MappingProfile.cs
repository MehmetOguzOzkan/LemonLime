using AutoMapper;
using LemonLime.DTOs.Auth;
using LemonLime.DTOs.Role;
using LemonLime.DTOs.User;
using LemonLime.Models;

namespace LemonLime.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Role, RoleRequest>().ReverseMap();
            CreateMap<Role, RoleResponse>().ReverseMap();

            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            CreateMap<User, UserResponse>();
        }
    }
}
