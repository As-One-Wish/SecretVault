using Hwj.Aow.Utils.CommonHelper.Helpers;
using Hwj.SecretVault.Application.ModuleAuthorization;
using Hwj.SecretVault.Infra.Entity.ModuleAuthorization.Dtos;
using Hwj.SecretVault.Infra.Entity.ModuleAuthorization.Params;
using Hwj.SecretVault.Infra.Entity.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hwj.SecretVault.HttpApi.Host.Controllers.ModuleAuthorization
{
    /// <summary>
    /// 授权控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        #region Initialize

        private readonly ISecretAppService _secretAppService;
        private readonly IJwtAppService _jwtAppService;

        /// <summary>
        /// 授权控制器构造函数
        /// </summary>
        /// <param name="secretAppService"></param>
        /// <param name="jwtAppService"></param>
        public AuthorizationController(ISecretAppService secretAppService, IJwtAppService jwtAppService)
        {
            _secretAppService = secretAppService;
            _jwtAppService = jwtAppService;
        }

        #endregion Initialize

        /// <summary>
        /// 获取Jwt授权数据
        /// </summary>
        /// <param name="jwtLoginParam"></param>
        /// <returns></returns>
        [HttpPost("CreateToken")]
        [ProducesResponseType(typeof(BaseResult<JwtAuthorizationDto?>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> CreateToken([FromBody] JwtLoginParam jwtLoginParam)
        {
            jwtLoginParam.Password = AesCryptHelper.DecryptByAesEcb(jwtLoginParam.Password);
            (JwtUserDto? JwtUserDto, string Msg) userInfo = await _secretAppService.GetJwtUserAsync(jwtLoginParam);
            if (userInfo.JwtUserDto == null)
                return Ok(new BaseResult(false, null, userInfo.Msg));
            else
            {
                JwtAuthorizationDto jwt = _jwtAppService.CreateJwt(userInfo.JwtUserDto.Value);
                return Ok(new BaseResult(!jwt.Equals(default), jwt));
            }
        }

        /// <summary>
        /// 刷新Jwt授权数据
        /// </summary>
        /// <param name="refreshCode"></param>
        /// <returns></returns>
        [HttpPost("RefreshToken")]
        [ProducesResponseType(typeof(BaseResult<JwtAuthorizationDto?>), StatusCodes.Status200OK)]
        public IActionResult RefreshToken([FromBody] long refreshCode)
        {
            // 用之前的ExpireTime作为验证码
            BaseResult<JwtAuthorizationDto?> refreshToken = _jwtAppService.RefreshJwt(Request.Headers["Authorization"], refreshCode);
            return Ok(refreshToken);
        }
    }
}