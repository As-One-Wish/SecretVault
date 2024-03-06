using Hwj.SecretVault.Infra.Entity.ModuleAuthorization.Params;
using Hwj.SecretVault.Infra.Repository.Databases.Entities;
using Hwj.SecretVault.Infra.Repository.Extension;
using Hwj.SecretVault.Infra.Repository.Shared;
using Hwj.SecretVault.Utils.CommonHelper.Helpers;

namespace Hwj.SecretVault.Infra.Repository.Databases.Repositories
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