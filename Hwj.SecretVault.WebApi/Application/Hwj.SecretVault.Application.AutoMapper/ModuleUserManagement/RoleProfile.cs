using AutoMapper;
using Info.Storage.Infra.Entity.ModuleUserManagement.Dtos;
using Info.Storage.Infra.Repository.Databases.Entities;

namespace Info.Storage.Application.AutoMapper.ModuleUserManagement
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