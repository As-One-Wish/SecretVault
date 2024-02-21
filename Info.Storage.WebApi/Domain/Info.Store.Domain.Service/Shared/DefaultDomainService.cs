using Info.Storage.Infa.Repository.Extension;
using Info.Storage.Infa.Repository.Shared;

namespace Info.Storage.Domain.Service.Shared
{
    /// <summary>
    /// 默认DomainService，只适用于单表的增删改查
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class DefaultDomainService<TEntity, TKey> : BaseDomainService<TEntity, TKey> where TEntity : class
    {
        private readonly IBaseSingleFreeSql<DbEnum> _singleFreeSql;

        public DefaultDomainService(IBaseSingleFreeSql<DbEnum> singleFreeSql) : base()
        {
            _singleFreeSql = singleFreeSql;
        }

        /// <summary>
        /// 初始化数据库枚举
        /// </summary>
        /// <param name="dbEnum"></param>
        public void InitDb(DbEnum dbEnum)
        {
            base.InitFreeSql(_singleFreeSql.Get(dbEnum));
        }
    }
}