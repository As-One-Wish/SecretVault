using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Infa.Repository.Extension;
using Info.Storage.Infa.Repository.Shared;

namespace Info.Storage.Infa.Repository.Databases.Repositories
{
    public class AppUserRepository : BaseRepositoryExtend<AppUser, long>
    {
        public AppUserRepository(IBaseSingleFreeSql<DbEnum> singleFreeSql) : base(singleFreeSql.Get(DbEnum.DbApp), null, null)
        {
        }
    }
}