using AutoMapper;
using Hwj.SecretVault.Infra.Entity.ModuleUserManagement.Dtos;
using Hwj.SecretVault.Infra.Repository.Databases.Entities;

namespace Hwj.SecretVault.Application.AutoMapper.ModuleUserManagement
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleDto, AppRole>().ForMember(dst => dst.RoleRemark, opt => opt.MapFrom(src => src.Remark));
            CreateMap<AppRole, RoleDto>().ForMember(dst => dst.Remark, opt => opt.MapFrom(src => src.RoleRemark));
        }
    }
}