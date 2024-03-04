using AutoMapper;
using Info.Storage.Domain.Service.ModuleUserManagement;
using Info.Storage.Infra.Entity.ModuleUserManagement.Dtos;
using Info.Storage.Infra.Entity.ModuleUserManagement.Params;
using Info.Storage.Infra.Entity.Shared.Attributes;
using Info.Storage.Infra.Entity.Shared.Constants;
using Info.Storage.Infra.Entity.Shared.Dtos;
using Info.Storage.Infra.Repository.Databases.Entities;
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

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="deleteRoleParam"></param>
        /// <returns></returns>
        Task<BaseResult> DelRole(DeleteRoleParam deleteRoleParam);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        Task<BaseResult> UpdateRole(RoleDto roleDto, bool responseData = false);

        /// <summary>
        /// 单一查询角色
        /// </summary>
        /// <param name="queryRoleParam"></param>
        /// <returns></returns>
        Task<BaseResult<RoleDto?>> GetRole(long roleId);

        /// <summary>
        /// 查询角色集合
        /// </summary>
        /// <param name="queryRoleParam"></param>
        /// <returns></returns>
        Task<BaseResult<IEnumerable<RoleDto>>> GetRoles(QueryRoleParam queryRoleParam);
    }

    [AutoInject(ServiceLifetime.Scoped, "app")]
    public class RoleService : IRoleService
    {
        #region Initialize

        private readonly IRoleDomainService _roleDomainService;
        private readonly IMapper _mapper;

        public RoleService(IRoleDomainService roleDomainService, IMapper mapper)
        {
            _roleDomainService = roleDomainService;
            _mapper = mapper;
        }

        #endregion Initialize

        #region Implements

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

        public async Task<BaseResult> DelRole(DeleteRoleParam deleteRoleParam)
        {
            try
            {
                BaseResult br = new BaseResult();
                int effectRows = await this._roleDomainService.DelRoleAsync(deleteRoleParam);
                br.IsSuccess = effectRows > 0;
                br.DataCount = effectRows;
                br.Message = !br.IsSuccess ? Msg.DbError : Msg.Success;

                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult(false, null, ex.Message));
            }
        }

        public async Task<BaseResult<RoleDto?>> GetRole(long roleId)
        {
            try
            {
                BaseResult<RoleDto?> br = new BaseResult<RoleDto?>();
                AppRole appRole = await this._roleDomainService.GetRoleAsync(roleId);
                bool isNullResult = appRole == null;
                br.IsSuccess = !isNullResult;
                if (!isNullResult)
                    br.Data = this._mapper.Map<AppRole, RoleDto>(appRole);
                br.Message = isNullResult ? Msg.DataNotFound : Msg.Success;
                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult<RoleDto?>(false, null, ex.Message));
            }
        }

        public async Task<BaseResult<IEnumerable<RoleDto>>> GetRoles(QueryRoleParam queryRoleParam)
        {
            try
            {
                BaseResult<IEnumerable<RoleDto>> br = new BaseResult<IEnumerable<RoleDto>>();
                (long DataCount, IEnumerable<AppRole> Data) result = await this._roleDomainService.GetRolesAsync(queryRoleParam);
                br.IsSuccess = result.Data != null;
                br.Message = !br.IsSuccess ? Msg.DataNotFound : Msg.Success;
                if (br.IsSuccess)
                {
                    br.Data = this._mapper.Map<IEnumerable<AppRole>, IEnumerable<RoleDto>>(result.Data);
                    br.DataCount = result.DataCount == -1 ? result.Data.Count() : result.DataCount;
                }

                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult<IEnumerable<RoleDto>>(false, Enumerable.Empty<RoleDto>(), ex.Message));
            }
        }

        public async Task<BaseResult> UpdateRole(RoleDto roleDto, bool responseData = false)
        {
            try
            {
                BaseResult br = new BaseResult();
                AppRole appRole = this._mapper.Map<RoleDto, AppRole>(roleDto);
                roleDto.UpdateTime = appRole.UpdateTime;

                int effectRows = await this._roleDomainService.UpdateRoleAsync(appRole);
                br.IsSuccess = effectRows > 0;
                br.DataCount = effectRows;
                if (responseData && br.IsSuccess)
                    br.Data = roleDto;
                br.Message = !br.IsSuccess ? Msg.DbError : Msg.Success;
                return br;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult(new BaseResult(false, null, ex.Message));
            }
        }

        #endregion Implements
    }
}