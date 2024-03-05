using Info.Storage.Application.Validator.ModuleUserManagement;
using Info.Storage.Domain.Service.ModuleAuthorization;
using Info.Storage.Infra.Entity.ModuleAuthorization.Dtos;
using Info.Storage.Infra.Entity.ModuleAuthorization.Params;
using Info.Storage.Infra.Entity.Shared.Attributes;
using Info.Storage.Utils.CommonHelper.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
                JwtLoginParamValidator validator = new JwtLoginParamValidator();
                var result = validator.Validate(jwtLoginParam);
                if (result.IsValid)
                    return await _secretDomainService.GetJwtUserAsync(jwtLoginParam);
                else
                    return await Task.FromResult<(JwtUserDto?, string)>((null, string.Join(";", result.Errors.Select(d => d.ErrorMessage).ToArray())));
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