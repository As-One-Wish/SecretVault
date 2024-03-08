using Hwj.SecretVault.Infra.Entity.Shared.Constants;
using Hwj.SecretVault.Infra.Repository.Databases.Entities;
using Hwj.Aow.Utils.RedisHelper;

namespace Hwj.SecretVault.Infra.Cache.ModuleInfoManagement
{
    /// <summary>
    /// 信息缓存静态类
    /// </summary>
    public static class InfoCache
    {
        /// <summary>
        /// 获取单一信息缓存
        /// </summary>
        /// <param name="infoId"></param>
        /// <returns></returns>
        public static async Task<AppInfo> GetInfoCacheAsync(long infoId)
        {
            return await RedisServerHelper.GetAsync<AppInfo>($"{CacheKeyPrefix.AppInfoCachePrefix}{infoId}");
        }

        /// <summary>
        /// 设置单一信息缓存
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static async Task<bool> SetInfoCacheAsync(AppInfo info)
        {
            return await RedisServerHelper.SetAsync($"{CacheKeyPrefix.AppInfoCachePrefix}{info.InfoId}", info);
        }

        /// <summary>
        /// 批量设置信息缓存
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public static long SetInfosCache(IEnumerable<AppInfo> infos)
        {
            var redisPipeDatas = RedisServerHelper.StartPipe(p =>
            {
                foreach (var info in infos)
                    p.Set($"{CacheKeyPrefix.AppInfoCachePrefix}{info.InfoId}", info);
            });

            return redisPipeDatas.Length;
        }

        /// <summary>
        /// 删除单一信息缓存
        /// </summary>
        /// <param name="infoId"></param>
        /// <returns></returns>
        public static async Task<long> DelInfoCacheAsync(long infoId)
        {
            return await RedisServerHelper.DelAsync($"{CacheKeyPrefix.AppInfoCachePrefix}{infoId}");
        }

        /// <summary>
        /// 批量删除信息缓存
        /// </summary>
        /// <param name="infoIds"></param>
        /// <returns></returns>
        public static long DelInfosCache(long[] infoIds)
        {
            var redisPipeDatas = RedisServerHelper.StartPipe(p =>
            {
                foreach (var infoId in infoIds)
                    p.Del($"{CacheKeyPrefix.AppInfoCachePrefix}{infoId}");
            });
            return redisPipeDatas.Length;
        }

        /// <summary>
        /// 删除所有信息缓存
        /// </summary>
        /// <returns></returns>
        public static async Task<long> DelAllInfoCacheAsync()
        {
            string[] strKeys = RedisServerHelper.Keys($"{CacheKeyPrefix.AppInfoCachePrefix}*");
            return await RedisServerHelper.DelAsync(strKeys);
        }
    }
}