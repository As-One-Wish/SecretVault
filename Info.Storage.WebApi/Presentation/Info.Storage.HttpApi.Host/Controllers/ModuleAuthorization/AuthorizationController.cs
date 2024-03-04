using Info.Storage.Application.ModuleAuthorization;
using Info.Storage.Infra.Entity.ModuleAuthorization.Dtos;
using Info.Storage.Infra.Entity.ModuleAuthorization.Params;
using Info.Storage.Infra.Entity.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Info.Storage.HttpApi.Host.Controllers.ModuleAuthorization
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

        public AuthorizationController(ISecretAppService secretAppService, IJwtAppService jwtAppService)
        {
            this._secretAppService = secretAppService;
            this._jwtAppService = jwtAppService;
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