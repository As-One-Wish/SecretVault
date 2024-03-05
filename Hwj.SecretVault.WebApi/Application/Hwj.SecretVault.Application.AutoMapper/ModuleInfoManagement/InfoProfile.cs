using AutoMapper;
using Info.Storage.Infra.Entity.ModuleInfoManagement.Dtos;
using Info.Storage.Infra.Repository.Databases.Entities;

namespace Info.Storage.Application.AutoMapper.ModuleInfoManagement
{
    public class InfoProfile : Profile
    {
        public InfoProfile()
        {
            CreateMap<AppInfo, InfoDto>().ForMember(dst => dst.Account, opt => opt.MapFrom(src => src.InfoAccount))
                .ForMember(dst => dst.Remark, opt => opt.MapFrom(src => src.InfoRemark))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.InfoContent));
            CreateMap<InfoDto, AppInfo>().ForMember(dst => dst.InfoAccount, opt => opt.MapFrom(src => src.Account))
                .ForMember(dst => dst.InfoRemark, opt => opt.MapFrom(src => src.Remark))
                .ForMember(dst => dst.InfoContent, opt => opt.MapFrom(src => src.Content));
        }
    }
}