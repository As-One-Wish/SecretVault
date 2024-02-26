using FreeSql;
using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Infa.Repository.Extension;
using Info.Storage.Infa.Repository.Shared;

namespace Info.Storage.Infa.Repository.Databases.Repositories
{
    public class AppRoleRepository : BaseRepositoryExtend<AppRole, long>
    {
        public AppRoleRepository(IBaseSingleFreeSql<DbEnum> singleFreeSql) : base(singleFreeSql.Get(DbEnum.DbApp), null)
        {
        }
    }
}