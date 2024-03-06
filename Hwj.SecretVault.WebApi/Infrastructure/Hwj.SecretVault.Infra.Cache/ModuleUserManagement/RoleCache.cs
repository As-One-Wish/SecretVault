using Hwj.SecretVault.Infra.Entity.Shared.Constants;
using Hwj.SecretVault.Infra.Repository.Databases.Entities;
using Hwj.SecretVault.Utils.RedisHelper;

namespace Hwj.SecretVault.Infra.Cache.ModuleUserManagement
{
    /// <summary>
    /// 角色缓存静态类
    /// </summary>
    public static class RoleCache
    {
        /// <summary>
        /// 获取单个角色缓存信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static async Task<AppRole> GetRoleCacheAsync(long roleId)
        {
            return await RedisServerHelper.GetAsync<AppRole>($"{CacheKeyPrefix.AppRoleCachePrefix}{roleId}");
        }

        /// <summary>
        /// 设置单个角色缓存信息
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static async Task<bool> SetRoleCacheAsync(AppRole role)
        {
            return await RedisServerHelper.SetAsync($"{CacheKeyPrefix.AppRoleCachePrefix}{role.RoleId}", role);
        }

        /// <summary>
        /// 批量设置角色缓存
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static long SetRolesCache(IEnumerable<AppRole> roles)
        {
            var redisPipeDatas = RedisServerHelper.StartPipe(p =>
            {
                foreach (var role in roles)
                    p.Set($"{CacheKeyPrefix.AppRoleCachePrefix}{role.RoleId}", role);
            });
            return redisPipeDatas.Length;
        }

        /// <summary>
        /// 删除角色缓存信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static async Task<long> DelRoleCacheAsync(long roleId)
        {
            return await RedisServerHelper.DelAsync($"{CacheKeyPrefix.AppRoleCachePrefix}{roleId}");
        }

        /// <summary>
        /// 批量删除角色缓存信息
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public static long DelRolesCache(long[] roleIds)
        {
            var redisPipeDatas = RedisServerHelper.StartPipe(p =>
            {
                foreach (var roleId in roleIds)
                    p.Del($"{CacheKeyPrefix.AppRoleCachePrefix}{roleId}");
            });

            return redisPipeDatas.Length;
        }

        /// <summary>
        /// 删除所有角色缓存信息
        /// </summary>
        /// <returns></returns>
        public static async Task<long> DelAllRoleCacheAsync()
        {
            string[] strKeys = RedisServerHelper.Keys($"{CacheKeyPrefix.AppRoleCachePrefix}*");
            return await RedisServerHelper.DelAsync(strKeys);
        }
    }
}