﻿using AutoMapper;
using Hwj.SecretVault.Infra.Entity.ModuleInfoManagement.Dtos;
using Hwj.SecretVault.Infra.Repository.Databases.Entities;

namespace Hwj.SecretVault.Application.AutoMapper.ModuleInfoManagement
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