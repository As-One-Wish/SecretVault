using Info.Storage.Domain.Service.Shared;
using Info.Storage.Infa.Entity.ModuleUserManagement.Params;
using Info.Storage.Infa.Entity.Shared.Attributes;
using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Infa.Repository.Databases.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Info.Storage.Domain.Service.ModuleUserManagement
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
            AppRole appRole = await this._appRoleRepository.InsertAsync(role);
            return appRole;
        }

        public async Task<int> DelRoleAsync(DeleteRoleParam deleteRoleParam)
        {
            int effectRows = -1;
            if (deleteRoleParam.RoleId != null)
                effectRows = await this._appRoleRepository.DeleteAsync(deleteRoleParam.RoleId.Value);
            if (deleteRoleParam.RoleIds != null && deleteRoleParam.RoleIds.Length > 0)
                effectRows = await this._appRoleRepository.DeleteAsync(id => deleteRoleParam.RoleIds.Contains(id.RoleId));

            return effectRows;
        }

        public async Task<AppRole> GetRoleAsync(long roleId)
        {
            AppRole result = await this._appRoleRepository.Where(d => d.RoleId == roleId).ToOneAsync();
            return result;
        }

        public async Task<(long, IEnumerable<AppRole>)> GetRolesAsync(QueryRoleParam queryRoleParam)
        {
            List<AppRole> lstResult = null;
            long dataCount = -1;
            var select = this._appRoleRepository.Select;

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
            int effectRows = await this._appRoleRepository.UpdateAsync(appRole);
            return effectRows;
        }

        #endregion Implements
    }
}