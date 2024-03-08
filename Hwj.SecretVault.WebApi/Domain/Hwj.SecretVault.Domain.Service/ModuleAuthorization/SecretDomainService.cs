using Hwj.SecretVault.Infra.Entity.ModuleAuthorization.Dtos;
using Hwj.SecretVault.Infra.Entity.ModuleAuthorization.Params;
using Hwj.SecretVault.Infra.Entity.Shared.Attributes;
using Hwj.SecretVault.Infra.Entity.Shared.Constants;
using Hwj.SecretVault.Infra.Repository.Databases.Entities;
using Hwj.SecretVault.Infra.Repository.Databases.Repositories;
using Hwj.Aow.Utils.CommonHelper.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Hwj.SecretVault.Domain.Service.ModuleAuthorization
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
    [AutoInject(ServiceLifetime.Scoped, "app")]
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

            bool userExists = await _appUserRepository.Select
                .AnyAsync(d => d.UserAccount == jwtLoginParam.Account && d.UserPwd == CryptHelper.Encrypt(jwtLoginParam.Password, "Info", true));
            if (userExists)
            {
                (AppUser appUser, AppRole appRole) userLoginRelatedInfo = await _appUserRepository.GetUserLoginFullInformation(jwtLoginParam);
                JwtUserDto jwtUserDto = new JwtUserDto
                {
                    UserName = userLoginRelatedInfo.appUser.UserName,
                    Account = userLoginRelatedInfo.appUser.UserAccount,
                    UserId = userLoginRelatedInfo.appUser.UserId,
                    RoleId = userLoginRelatedInfo.appUser.RoleId,
                    RoleName = userLoginRelatedInfo.appRole.RoleName
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