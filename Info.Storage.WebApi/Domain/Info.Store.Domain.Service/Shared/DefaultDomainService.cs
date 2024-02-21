using Info.Storage.Infa.Repository.Shared;

namespace Info.Storage.Domain.Service.Shared
{
    public class DefaultDomainService<TEntity, TKey> : BaseDomainService<TEntity, TKey> where TEntity : class
    {
        private readonly SingleFreeSql _singleFreeSql;
    }
}