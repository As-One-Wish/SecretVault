using AutoMapper;
using Info.Storage.Infa.Entity.ModuleUserManagement.Dtos;
using Info.Storage.Infa.Repository.Databases.Entities;

namespace Info.Storage.Application.AutoMapper.ModuleUserManagement
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleDto, AppRole>().ForMember(dst => dst.RoleRemark, opt => opt.MapFrom(src => src.Remark));
            CreateMap<AppRole, RoleDto>();
        }
    }
}