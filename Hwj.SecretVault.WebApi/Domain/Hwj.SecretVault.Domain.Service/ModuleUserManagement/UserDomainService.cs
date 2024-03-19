using Hwj.SecretVault.Domain.Service.Shared;
using Hwj.SecretVault.Infra.Cache.ModuleUserManagement;
using Hwj.SecretVault.Infra.Entity.ModuleUserManagement.Dtos;
using Hwj.SecretVault.Infra.Entity.ModuleUserManagement.Params;
using Hwj.SecretVault.Infra.Entity.Shared.Attributes;
using Hwj.SecretVault.Infra.Repository.Databases.Entities;
using Hwj.SecretVault.Infra.Repository.Databases.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Hwj.SecretVault.Domain.Service.ModuleUserManagement
{
    /// <summary>
    /// 用户管理领域接口
    /// </summary>
    public interface IUserDomainService
    {
        /// <summary>
        /// 异步添加用户
        /// </summary>
        /// <param name="appUser">用户数据库实体</param>
        /// <returns>数据库实体</returns>
        Task<AppUser> AddUserAsync(AppUser appUser);

        /// <summary>
        /// 异步删除用户
        /// </summary>
        /// <param name="deleteUserParam">用户删除参数</param>
        /// <returns></returns>
        Task<int> DelUserAsync(DeleteUserParam deleteUserParam);

        /// <summary>
        /// 物理删除用户
        /// </summary>
        /// <returns></returns>
        Task<int> DelUserPhysicallyAsync();

        /// <summary>
        /// 异步更新用户
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        Task<int> UpdateUserAsync(AppUser appUser);

        /// <summary>
        /// 异步查询单一用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AppUser> GetUserAsync(long userId);

        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        Task<bool> IsUserAccountExistAsync(string userAccount);

        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <param name="queryUserParam"></param>
        /// <returns>(数据条数，实体集合)</returns>
        Task<(long, IEnumerable<UserDto>)> GetUsersAsync(QueryUserParam queryUserParam);

        /// <summary>
        /// 获取用户登录相关信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<(AppUser, AppRole)> GetUserLoginRelatedAsync(long userId);
    }

    /// <summary>
    /// 用户管理领域实现
    /// </summary>
    [AutoInject(ServiceLifetime.Scoped, "app")]
    public class UserDomainService : BaseDomainService<AppUser, long>, IUserDomainService
    {
        #region Initialize

        private readonly AppUserRepository _appUserRepository;

        public UserDomainService(AppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        #endregion Initialize

        #region Implements

        public async Task<AppUser> AddUserAsync(AppUser appUser)
        {
            AppUser oAppUser = await _appUserRepository.InsertAsync(appUser);
            if (oAppUser != null)
                await UserCache.SetUserCacheAsync(oAppUser);
            return oAppUser;
        }

        public async Task<int> DelUserAsync(DeleteUserParam deleteUserParam)
        {
            List<AppUser> usersToDel = new List<AppUser>();
            if (deleteUserParam.UserId != null)
            {
                usersToDel = await _appUserRepository.Where(user => user.UserId == deleteUserParam.UserId.Value && !user.IsDeleted).ToListAsync();
                if (usersToDel.Count > 0)
                    await UserCache.DelUserCacheAsync(deleteUserParam.UserId.Value);
            }
            if (deleteUserParam.UserIds != null && deleteUserParam.UserIds.Length > 0)
            {
                usersToDel = await _appUserRepository.Where(user => deleteUserParam.UserIds.Contains(user.UserId) && !user.IsDeleted).ToListAsync();
                if (usersToDel.Count > 0)
                    UserCache.DelUsersCache(deleteUserParam.UserIds);
            }
            if (usersToDel != null && usersToDel.Count() > 0)
                foreach (AppUser user in usersToDel)
                {
                    user.IsDeleted = true;
                    await _appUserRepository.UpdateAsync(user);
                }
            return usersToDel.Count > 0 ? usersToDel.Count : -1;
        }

        public async Task<int> UpdateUserAsync(AppUser appUser)
        {
            int effectRows = await _appUserRepository.UpdateAsync(appUser);
            if (effectRows > 0)
                await UserCache.SetUserCacheAsync(appUser);

            return effectRows;
        }

        public async Task<AppUser> GetUserAsync(long userId)
        {
            // 检查缓存是否存在
            AppUser result = await UserCache.GetUserCacheAsync(userId);
            if (result == null)
            {
                // 不存在则查询数据库
                result = await _appUserRepository.Where(d => d.UserId == userId && !d.IsDeleted).ToOneAsync();
                await UserCache.SetUserCacheAsync(result);
            }
            return result;
        }

        public async Task<bool> IsUserAccountExistAsync(string userAccount)
        {
            return await _appUserRepository.Select.AnyAsync(d => d.UserAccount == userAccount);
        }

        public async Task<(long, IEnumerable<UserDto>)> GetUsersAsync(QueryUserParam queryUserParam)
        {
            List<UserDto> lstResult = null;
            long dataCount = -1;
            var oSelect = _appUserRepository.Orm.Select<AppUser, AppRole>().LeftJoin((a, b) => a.RoleId == b.RoleId);

            #region 条件查询

            oSelect.WhereIf(!string.IsNullOrWhiteSpace(queryUserParam.SearchText), (a, b) => a.UserName.Contains(queryUserParam.SearchText));
            oSelect.WhereIf(queryUserParam.UserId != null, (a, b) => a.UserId.Equals(queryUserParam.UserId));
            oSelect.Where((a, b) => !a.IsDeleted);

            #endregion 条件查询

            #region 排序

            // 多表关联，加入默认表别名进行排序
            oSelect.OrderBy(!string.IsNullOrWhiteSpace(queryUserParam.OrderBy),
                queryUserParam.OrderBy + (string.IsNullOrWhiteSpace(queryUserParam.OrderByType) ? " asc" : " " + queryUserParam.OrderByType));

            #endregion 排序

            #region 分页

            if (queryUserParam.PageIndex != null && queryUserParam.PageSize != null)
            {
                dataCount = oSelect.Count();
                oSelect.Page(queryUserParam.PageIndex.Value, queryUserParam.PageSize.Value);
            }

            #endregion 分页

            lstResult = await oSelect.ToListAsync((a, b) => new UserDto
            {
                Avatar = queryUserParam.ShowAvatar ? a.UserAvatar : null,
                RoleName = b.RoleName,
                Account = a.UserAccount,
                Password = a.UserPwd,
                Phone = a.UserPhone
            });
            return (dataCount, lstResult);
        }

        public async Task<int> DelUserPhysicallyAsync()
        {
            int effectRows = await _appUserRepository.DeleteAsync(user => user.IsDeleted);
            return effectRows;
        }

        public async Task<(AppUser, AppRole)> GetUserLoginRelatedAsync(long userId)
        {
            // 优先读取缓存
            (AppUser appUser, AppRole appRole) result = UserCache.GetUserLoginRelatedCache(userId);
            if (result.appUser == null || result.appRole == null)
                // 缓存不存在则读数据库
                return await this._appUserRepository.GetUserLoginRelated(userId);
            else
                return result;
        }

        #endregion Implements
    }
}