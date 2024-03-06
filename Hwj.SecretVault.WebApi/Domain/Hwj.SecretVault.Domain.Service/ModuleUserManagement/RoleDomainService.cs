using Hwj.SecretVault.Domain.Service.Shared;
using Hwj.SecretVault.Infra.Cache.ModuleUserManagement;
using Hwj.SecretVault.Infra.Entity.ModuleUserManagement.Params;
using Hwj.SecretVault.Infra.Entity.Shared.Attributes;
using Hwj.SecretVault.Infra.Repository.Databases.Entities;
using Hwj.SecretVault.Infra.Repository.Databases.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Hwj.SecretVault.Domain.Service.ModuleUserManagement
{
    /// <summary>
    /// 角色管理领域接口
    /// </summary>
    public interface IRoleDomainService
    {
        /// <summary>
        /// 异步添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<AppRole> AddRoleAsync(AppRole role);

        /// <summary>
        /// 异步删除角色
        /// </summary>
        /// <param name="deleteRoleParam"></param>
        /// <returns></returns>
        Task<int> DelRoleAsync(DeleteRoleParam deleteRoleParam);

        /// <summary>
        /// 异步更新角色
        /// </summary>
        /// <param name="appRole"></param>
        /// <returns></returns>
        Task<int> UpdateRoleAsync(AppRole appRole);

        /// <summary>
        /// 异步查询单一角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<AppRole> GetRoleAsync(long roleId);

        /// <summary>
        /// 查询角色集合
        /// </summary>
        /// <returns></returns>
        Task<(long, IEnumerable<AppRole>)> GetRolesAsync(QueryRoleParam queryRoleParam);
    }

    /// <summary>
    /// 角色管理领域实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped, "app")]
    public class RoleDomainService : BaseDomainService<AppRole, long>, IRoleDomainService
    {
        #region Initialize

        private readonly AppRoleRepository _appRoleRepository;

        public RoleDomainService(AppRoleRepository appRoleRepository)
        {
            _appRoleRepository = appRoleRepository;
        }

        #endregion Initialize

        #region Implements

        public async Task<AppRole> AddRoleAsync(AppRole role)
        {
            AppRole appRole = await _appRoleRepository.InsertAsync(role);
            if (appRole != null)
                await RoleCache.SetRoleCacheAsync(appRole);
            return appRole;
        }

        public async Task<int> DelRoleAsync(DeleteRoleParam deleteRoleParam)
        {
            int effectRows = -1;
            if (deleteRoleParam.RoleId != null)
            {
                effectRows = await _appRoleRepository.DeleteAsync(deleteRoleParam.RoleId.Value);
                if (effectRows > 0)
                    await RoleCache.DelRoleCacheAsync(deleteRoleParam.RoleId.Value);
            }
            if (deleteRoleParam.RoleIds != null && deleteRoleParam.RoleIds.Length > 0)
            {
                effectRows = await _appRoleRepository.DeleteAsync(id => deleteRoleParam.RoleIds.Contains(id.RoleId));
                if (effectRows > 0)
                    RoleCache.DelRolesCache(deleteRoleParam.RoleIds);
            }

            return effectRows;
        }

        public async Task<AppRole> GetRoleAsync(long roleId)
        {
            var result = await RoleCache.GetRoleCacheAsync(roleId);
            if (result == null)
            {
                result = await _appRoleRepository.Where(d => d.RoleId == roleId).ToOneAsync();
                if (result != null)
                    await RoleCache.SetRoleCacheAsync(result);
            }

            return result;
        }

        public async Task<(long, IEnumerable<AppRole>)> GetRolesAsync(QueryRoleParam queryRoleParam)
        {
            List<AppRole> lstResult = null;
            long dataCount = -1;
            var select = _appRoleRepository.Orm.Select<AppRole>();

            #region 条件查询

            select.WhereIf(!string.IsNullOrWhiteSpace(queryRoleParam.SearchText), d => d.RoleName.Contains(queryRoleParam.SearchText));
            select.WhereIf(queryRoleParam.RoleId != null, d => d.RoleId.Equals(queryRoleParam.RoleId));

            #endregion 条件查询

            #region 排序

            select.OrderBy(!string.IsNullOrWhiteSpace(queryRoleParam.OrderBy),
                "a." + queryRoleParam.OrderBy + (string.IsNullOrWhiteSpace(queryRoleParam.OrderByType) ?
                " asc" : queryRoleParam.OrderByType == "ascend" ? " asc" : " desc"));

            #endregion 排序

            #region 分页

            if (queryRoleParam.PageIndex != null && queryRoleParam.PageSize != null)
                select.Count(out dataCount).Page(queryRoleParam.PageIndex.Value, queryRoleParam.PageSize.Value);

            #endregion 分页

            lstResult = await select.ToListAsync();
            return (dataCount, lstResult);
        }

        public async Task<int> UpdateRoleAsync(AppRole appRole)
        {
            int effectRows = await _appRoleRepository.UpdateAsync(appRole);
            if (effectRows > 0)
                await RoleCache.SetRoleCacheAsync(appRole);
            return effectRows;
        }

        #endregion Implements
    }
}