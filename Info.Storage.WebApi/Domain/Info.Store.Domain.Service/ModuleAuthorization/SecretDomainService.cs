using Info.Storage.Infa.Entity.ModuleAuthorization.Dtos;
using Info.Storage.Infa.Entity.ModuleAuthorization.Params;
using Info.Storage.Infa.Entity.Shared.Constants;
using Info.Storage.Infa.Repository.Databases.Entities;
using Info.Storage.Infa.Repository.Databases.Repositories;
using Info.Storage.Utils.CommonHelper.Helpers;

namespace Info.Storage.Domain.Service.ModuleAuthorization
{
    /// <summary>
    /// 安全领域接口
    /// </summary>
    public interface ISecretDomainService
    {
        /// <summary>
        /// 根据账户名、密码获取用户实体
        /// </summary>
        /// <param name="jwtLoginParam"></param>
        /// <returns></returns>
        Task<(JwtUserDto?, string)> GetJwtUserAsync(JwtLoginParam jwtLoginParam);
    }

    /// <summary>
    /// 安全领域实现
    /// </summary>
    public class SecretDomainService : ISecretDomainService
    {
        #region Initialize

        public readonly AppUserRepository _appUserRepository;

        public SecretDomainService(AppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        #endregion Initialize

        #region Implements

        public async Task<(JwtUserDto?, string)> GetJwtUserAsync(JwtLoginParam jwtLoginParam)
        {
            (JwtUserDto? jwtUserDto, string Message) result = (null, "");

            if (jwtLoginParam == default) return (null, Msg.ParamError);

            AppUser appUser = await this._appUserRepository
                    .Where(d => d.UserName == jwtLoginParam.UserName && d.UserPwd == CryptHelper.Encrypt(jwtLoginParam.Password, "Info", true))
                    .ToOneAsync();
            if (appUser != null)
            {
                JwtUserDto jwtUserDto = new JwtUserDto
                {
                    UserName = appUser.UserName,
                    UserId = appUser.UserId,
                    RoleId = appUser.RoleId
                };
                result.jwtUserDto = jwtUserDto;
            }
            else
            {
                result.Message = $"{Msg.NoAccount}或{Msg.PasswordError}";
            }
            return result;
        }

        #endregion Implements
    }
}