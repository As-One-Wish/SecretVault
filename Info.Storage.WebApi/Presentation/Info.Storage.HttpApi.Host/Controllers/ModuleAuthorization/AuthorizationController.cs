using Info.Storage.Application.ModuleAuthorization;

namespace Info.Storage.HttpApi.Host.Controllers.ModuleAuthorization
{
    public class AuthorizationController
    {
        #region Initialize

        private readonly ISecretAppService _secretAppService;

        public AuthorizationController(ISecretAppService secretAppService)
        {
            this._secretAppService = secretAppService;
        }

        #endregion Initialize
    }
}