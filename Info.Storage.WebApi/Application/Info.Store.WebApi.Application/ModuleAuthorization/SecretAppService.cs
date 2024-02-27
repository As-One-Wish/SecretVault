using Info.Storage.Domain.Service.ModuleAuthorization;
using Info.Storage.Infa.Entity.ModuleAuthorization.Dtos;
using Info.Storage.Infa.Entity.ModuleAuthorization.Params;
using Info.Storage.Infa.Entity.Shared.Attributes;
using Info.Storage.Utils.CommonHelper.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Info.Storage.Application.ModuleAuthorization
{
    public interface ISecretAppService
    {
        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="jwtLoginParam"></param>
        /// <returns></returns>
        Task<(JwtUserDto?, string)> GetJwtUserAsync(JwtLoginParam jwtLoginParam);
    }

    [AutoInject(ServiceLifetime.Scoped, "app")]
    public class SecretAppService : ISecretAppService
    {
        #region Initialize

        private readonly ISecretDomainService _secretDomainService;

        public SecretAppService(ISecretDomainService secretDomainService)
        {
            _secretDomainService = secretDomainService;
        }

        #endregion Initialize

        #region Implements

        public async Task<(JwtUserDto?, string)> GetJwtUserAsync(JwtLoginParam jwtLoginParam)
        {
            try
            {
                // TODO 参数校验
                return await _secretDomainService.GetJwtUserAsync(jwtLoginParam);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return await Task.FromResult<(JwtUserDto?, string)>((null, ex.Message));
            }
        }

        #endregion Implements
    }
}