using Info.Storage.Infra.Repository.Databases.Entities;
using Info.Storage.Infra.Repository.Extension;
using Info.Storage.Infra.Repository.Shared;

namespace Info.Storage.Infra.Repository.Databases.Repositories
{
    public class AppInfoRepository : BaseRepositoryExtend<AppInfo, long>
    {
        public AppInfoRepository(IBaseSingleFreeSql<DbEnum> singleFreeSql) : base(singleFreeSql.Get(DbEnum.DbApp), null, null)
        {
        }
    }
}