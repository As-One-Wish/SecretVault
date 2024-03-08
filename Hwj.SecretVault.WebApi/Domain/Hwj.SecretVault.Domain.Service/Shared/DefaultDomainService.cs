using Hwj.Aow.Infra.Repository.Extension;
using Hwj.SecretVault.Infra.Repository.Shared;

namespace Hwj.SecretVault.Domain.Service.Shared
{
    /// <summary>
    /// 默认DomainService，只适用于单表的增删改查
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class DefaultDomainService<TEntity, TKey> : BaseDomainService<TEntity, TKey> where TEntity : class
    {
        #region Initialize

        private readonly IBaseSingleFreeSql<DbEnum> _singleFreeSql;

        public DefaultDomainService(IBaseSingleFreeSql<DbEnum> singleFreeSql) : base()
        {
            _singleFreeSql = singleFreeSql;
        }

        #endregion Initialize

        /// <summary>
        /// 初始化数据库枚举
        /// </summary>
        /// <param name="dbEnum"></param>
        public void InitDb(DbEnum dbEnum)
        {
            InitFreeSql(_singleFreeSql.Get(dbEnum));
        }
    }
}