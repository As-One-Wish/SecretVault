using Info.Storage.Infa.Entity.ModuleAuthorization.Params;
using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Infa.Repository.Extension;
using Info.Storage.Infa.Repository.Shared;
using Info.Storage.Utils.CommonHelper.Helpers;

namespace Info.Storage.Infa.Repository.Databases.Repositories
{
    public class AppUserRepository : BaseRepositoryExtend<AppUser, long>
    {
        public AppUserRepository(IBaseSingleFreeSql<DbEnum> singleFreeSql) : base(singleFreeSql.Get(DbEnum.DbApp), null, null)
        {
        }

        /// <summary>
        /// 获取用于Token的用户登录的全部信息
        /// </summary>
        /// <param name="jwtLoginParam"></param>
        /// <returns></returns>
        public async Task<(AppUser, AppRole)> GetUserLoginFullInformation(JwtLoginParam jwtLoginParam)
        {
            dynamic obj = await Orm.Select<AppUser, AppRole>()
                .LeftJoin((a, b) => a.RoleId == b.RoleId)
                .Where((a, b) => a.UserAccount == jwtLoginParam.Account && a.UserPwd == CryptHelper.Encrypt(jwtLoginParam.Password, "Info", true))
                .ToOneAsync((a, b) => new { a, b });
            return (obj.a, obj.b);
        }
    }
}