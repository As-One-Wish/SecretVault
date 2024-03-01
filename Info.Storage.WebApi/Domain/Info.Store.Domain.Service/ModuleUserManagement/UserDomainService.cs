using Info.Storage.Domain.Service.Shared;
using Info.Storage.Infa.Entity.ModuleUserManagement.Dtos;
using Info.Storage.Infa.Entity.ModuleUserManagement.Params;
using Info.Storage.Infa.Entity.Shared.Attributes;
using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Infa.Repository.Databases.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Info.Storage.Domain.Service.ModuleUserManagement
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
            this._appUserRepository = appUserRepository;
        }

        #endregion Initialize

        #region Implements

        public async Task<AppUser> AddUserAsync(AppUser appUser)
        {
            AppUser oAppUser = await this._appUserRepository.InsertAsync(appUser);
            // TODO 设置缓存
            return oAppUser;
        }

        public async Task<int> DelUserAsync(DeleteUserParam deleteUserParam)
        {
            List<AppUser> usersToDel = new List<AppUser>();
            if (deleteUserParam.UserId != null)
            {
                usersToDel = await this._appUserRepository.Where(user => user.UserId == deleteUserParam.UserId.Value && !user.IsDeleted).ToListAsync();
                // TODO 缓存
            }
            if (deleteUserParam.UserIds != null && deleteUserParam.UserIds.Length > 0)
            {
                usersToDel = await this._appUserRepository.Where(user => deleteUserParam.UserIds.Contains(user.UserId) && !user.IsDeleted).ToListAsync();
                // TODO 缓存
            }
            if (usersToDel != null && usersToDel.Count() > 0)
                foreach (AppUser user in usersToDel)
                {
                    user.IsDeleted = true;
                    await this._appUserRepository.UpdateAsync(user);
                }
            return usersToDel == null ? -1 : usersToDel.Count;
        }

        public async Task<int> UpdateUserAsync(AppUser appUser)
        {
            int effectRows = await this._appUserRepository.UpdateAsync(appUser);
            // TODO 缓存

            return effectRows;
        }

        public async Task<AppUser> GetUserAsync(long userId)
        {
            // TODO 缓存
            AppUser result = await this._appUserRepository.Where(d => d.UserId == userId && !d.IsDeleted).ToOneAsync();
            return result;
        }

        public async Task<bool> IsUserAccountExistAsync(string userAccount)
        {
            return await this._appUserRepository.Select.AnyAsync(d => d.UserAccount == userAccount);
        }

        public async Task<(long, IEnumerable<UserDto>)> GetUsersAsync(QueryUserParam queryUserParam)
        {
            List<UserDto> lstResult = null;
            long dataCount = -1;
            var oSelect = this._appUserRepository.Select;

            #region 条件查询

            oSelect.WhereIf(!string.IsNullOrWhiteSpace(queryUserParam.SearchText), d => d.UserName.Contains(queryUserParam.SearchText));
            oSelect.WhereIf(queryUserParam.UserId != null, d => d.UserId.Equals(queryUserParam.UserId));
            oSelect.Where(d => !d.IsDeleted);

            #endregion 条件查询

            #region 排序

            // 多表关联，加入默认表别名进行排序
            oSelect.OrderBy(!string.IsNullOrWhiteSpace(queryUserParam.OrderBy),
                "a." + queryUserParam.OrderBy + (string.IsNullOrWhiteSpace(queryUserParam.OrderByType) ?
                " asc" : queryUserParam.OrderByType == "ascend" ? " asc" : " desc"));

            #endregion 排序

            #region 分页

            if (queryUserParam.PageIndex != null && queryUserParam.PageSize != null)
                oSelect.Count(out dataCount).Page(queryUserParam.PageIndex.Value, queryUserParam.PageSize.Value);

            #endregion 分页

            // TODO AutoMapper转换
            lstResult = await oSelect.From<AppRole>((a, b) => a.LeftJoin(a => a.RoleId == b.RoleId))
                .ToListAsync((a, b) => new UserDto
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
            int effectRows = await this._appUserRepository.DeleteAsync(user => user.IsDeleted);
            return effectRows;
        }

        #endregion Implements
    }
}