using AutoMapper;
using Hwj.SecretVault.Infra.Entity.ModuleUserManagement.Dtos;
using Hwj.SecretVault.Infra.Repository.Databases.Entities;
using Hwj.Aow.Utils.CommonHelper.Helpers;

namespace Hwj.SecretVault.Application.AutoMapper.ModuleUserManagement
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // AppUser -> UserDto
            CreateMap<AppUser, UserDto>().ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.UserAccount))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.UserPwd))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.UserAvatar))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.UserPhone));
            // UserDto -> AppUser
            CreateMap<UserDto, AppUser>().ForMember(dest => dest.UserAccount, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.UserPhone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.UserPwd, opt => opt.MapFrom(src => CryptHelper.Encrypt(src.Password, "Info", true)))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId));
        }
    }
}