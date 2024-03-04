namespace Info.Storage.Infra.Entity.Shared.Constants
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
    }
}