using Info.Storage.Domain.Service.Shared;
using Info.Storage.Infa.Entity.ModuleUserManagement.Params;
using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Infa.Repository.Databases.Repositories;

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

    public class RoleDomainService : BaseDomainService<AppRole, long>, IRoleDomainService
    {
        private readonly AppRoleRepository _appRoleRepository;

        public RoleDomainService(AppRoleRepository appRoleRepository)
        {
            _appRoleRepository = appRoleRepository;
        }

        public Task<AppRole> AddRoleAsync(AppRole role)
        {
            throw new NotImplementedException();
        }

        public Task<int> DelRoleAsync(DeleteRoleParam deleteRoleParam)
        {
            throw new NotImplementedException();
        }

        public Task<AppRole> GetRoleAsync(long roleId)
        {
            throw new NotImplementedException();
        }

        public Task<(long, IEnumerable<AppRole>)> GetRolesAsync(QueryRoleParam queryRoleParam)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateRoleAsync(AppRole appRole)
        {
            throw new NotImplementedException();
        }
    }
}