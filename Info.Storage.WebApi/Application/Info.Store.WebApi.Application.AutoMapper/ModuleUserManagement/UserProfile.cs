using AutoMapper;
using Info.Storage.Infa.Entity.ModuleUserManagement.Dtos;
using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Utils.CommonHelper.Helpers;

namespace Info.Storage.Application.AutoMapper.ModuleUserManagement
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserDto>();
            // UserDto -> AppUser
            CreateMap<UserDto, AppUser>().ForMember(dest => dest.UserAccount, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.UserPhone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.UserPwd, opt => opt.MapFrom(src => CryptHelper.Encrypt(src.Password, "Info", true)))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId));
        }
    }
}