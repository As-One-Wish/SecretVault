namespace Hwj.SecretVault.Infra.Entity.Shared.Constants
{
    public class CacheKeyPrefix
    {
        /// <summary>
        /// 用户缓存前缀
        /// </summary>
        public static readonly string AppUserCachePrefix = "DomainServiceCache:AppUser:User_";

        /// <summary>
        /// 角色缓存前缀
        /// </summary>
        public static readonly string AppRoleCachePrefix = "DomainServiceCache:AppRole:Role_";

        /// <summary>
        /// 信息缓存前缀
        /// </summary>
        public static readonly string AppInfoCachePrefix = "DomainServiceCache:AppInfo:Info_";
    }
}