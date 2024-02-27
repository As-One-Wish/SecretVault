using AutoMapper;
using Info.Storage.Domain.Service.ModuleUserManagement;
using Info.Storage.Infa.Entity.ModuleUserManagement.Dtos;
using Info.Storage.Infa.Entity.Shared.Attributes;
using Info.Storage.Infa.Entity.Shared.Constants;
using Info.Storage.Infa.Entity.Shared.Dtos;
using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Utils.CommonHelper.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Info.Storage.Application.ModuleUserManagement
{
    public interface IRoleService
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleDto"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        Task<BaseResult<RoleDto?>> AddRole(RoleDto roleDto, bool responseData = false);
    }

    [AutoInject(ServiceLifetime.Scoped, "app")]
    public class RoleService : IRoleService
    {
        private readonly IRoleDomainService _roleDomainService;
        private readonly IMapper _mapper;

        public RoleService(IRoleDomainService roleDomainService, IMapper mapper)
        {
            _roleDomainService = roleDomainService;
            _mapper = mapper;
        }

        public async Task<BaseResult<RoleDto?>> AddRole(RoleDto roleDto, bool responseData = false)
        {
            try
            {
                BaseResult<RoleDto?> br = new BaseResult<RoleDto?>();
                roleDto.RoleId = Yitter.IdGenerator.YitIdHelper.NextId();

                AppRole appRole = this._mapper.Map<RoleDto, AppRole>(roleDto);
                AppRole result = await this._roleDomainService.AddRoleAsync(appRole);

                bool isNullResult = result == null;
                br.IsSuccess = !isNullResult;
                if (responseData && !isNullResult)
                    br.Data = roleDto;
                br.Message = isNullResult ? Msg.DbError : Msg.Success;
                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult<RoleDto?>(false, null, ex.Message));
            }
        }
    }
}