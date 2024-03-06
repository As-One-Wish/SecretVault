using Hwj.SecretVault.Infra.Entity.Shared.Constants;
using Hwj.SecretVault.Infra.Repository.Databases.Entities;
using Hwj.SecretVault.Utils.RedisHelper;

namespace Hwj.SecretVault.Infra.Cache.ModuleUserManagement
{
    /// <summary>
    /// 用户缓存静态类
    /// </summary>
    public static class UserCache
    {
        /// <summary>
        /// 获取单个用户缓存信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<AppUser> GetUserCacheAsync(long userId)
        {
            return await RedisServerHelper.GetAsync<AppUser>($"{CacheKeyPrefix.AppUserCachePrefix}{userId}");
        }

        /// <summary>
        /// 设置用户缓存信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<bool> SetUserCacheAsync(AppUser user)
        {
            return await RedisServerHelper.SetAsync($"{CacheKeyPrefix.AppUserCachePrefix}{user.UserId}", user);
        }

        /// <summary>
        /// 批量设置用户缓存信息
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static long SetUsersCache(IEnumerable<AppUser> users)
        {
            var redisPipeDatas = RedisServerHelper.StartPipe(p =>
            {
                foreach (var user in users)
                    p.Set($"{CacheKeyPrefix.AppUserCachePrefix}{user.UserId}", user);
            });

            return redisPipeDatas.Length;
        }

        /// <summary>
        /// 删除用户缓存信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<long> DelUserCacheAsync(long userId)
        {
            return await RedisServerHelper.DelAsync($"{CacheKeyPrefix.AppUserCachePrefix}{userId}");
        }

        /// <summary>
        /// 批量删除用户缓存信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public static long DelUsersCache(long[] userIds)
        {
            var redisPipeDatas = RedisServerHelper.StartPipe(p =>
            {
                foreach (var userId in userIds)
                    p.Del($"{CacheKeyPrefix.AppUserCachePrefix}{userId}");
            });
            return redisPipeDatas.Length;
        }

        /// <summary>
        /// 删除所有用户缓存信息
        /// </summary>
        /// <returns></returns>
        public static async Task<long> DelAllUserCacheAsync()
        {
            string[] strKeys = RedisServerHelper.Keys($"{CacheKeyPrefix.AppUserCachePrefix}*");
            return await RedisServerHelper.DelAsync(strKeys);
        }

        /// <summary>
        /// 设置用户登录获取完整信息
        /// </summary>
        /// <param name="userLoginFullInfo"></param>
        /// <returns></returns>
        public static long SetUserLoginFullInformationCache((AppUser appUser, AppRole appRole) userLoginFullInfo)
        {
            var redisPipeDatas = RedisServerHelper.StartPipe(p =>
            {
                if (userLoginFullInfo.appUser != null)
                    p.Set($"{CacheKeyPrefix.AppUserCachePrefix}{userLoginFullInfo.appUser.UserId}", userLoginFullInfo.appUser);
                if (userLoginFullInfo.appRole != null)
                    p.Set($"{CacheKeyPrefix.AppRoleCachePrefix}{userLoginFullInfo.appRole.RoleId}", userLoginFullInfo.appRole);
            });
            return redisPipeDatas.Length;
        }
    }
}