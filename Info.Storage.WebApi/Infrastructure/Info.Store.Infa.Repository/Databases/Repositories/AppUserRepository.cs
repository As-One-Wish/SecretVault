using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Infa.Repository.Extension;

namespace Info.Storage.Infa.Repository.Databases.Repositories
{
    public class AppUserRepository : BaseRepositoryExtend<AppUser, long>
    {
        public AppUserRepository(IFreeSql fsql) : base(fsql, null, null)
        {
        }
    }
}