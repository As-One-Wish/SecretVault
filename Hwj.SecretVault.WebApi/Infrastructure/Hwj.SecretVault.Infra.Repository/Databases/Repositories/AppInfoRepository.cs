using Hwj.SecretVault.Infra.Repository.Databases.Entities;
using Hwj.SecretVault.Infra.Repository.Extension;
using Hwj.SecretVault.Infra.Repository.Shared;

namespace Hwj.SecretVault.Infra.Repository.Databases.Repositories
{
    public class AppInfoRepository : BaseRepositoryExtend<AppInfo, long>
    {
        public AppInfoRepository(IBaseSingleFreeSql<DbEnum> singleFreeSql) : base(singleFreeSql.Get(DbEnum.DbApp), null, null)
        {
        }
    }
}