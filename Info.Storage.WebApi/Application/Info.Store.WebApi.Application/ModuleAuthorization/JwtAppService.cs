using Info.Storage.Infa.Entity.ModuleAuthorization.Dtos;
using Info.Storage.Infa.Entity.Shared.Attributes;
using Info.Storage.Utils.CommonHelper.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Info.Storage.Application.ModuleAuthorization
{
    /// <summary>
    /// Jwt Token管理接口
    /// </summary>
    public interface IJwtAppService
    {
        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="jwtUserDto"></param>
        /// <returns>Jwt授权结果Dto</returns>
        JwtAuthorizationDto CreateJwt(JwtUserDto jwtUserDto);
    }

    [AutoInject(ServiceLifetime.Scoped, "app")]
    public class JwtAppService : IJwtAppService
    {
        #region Initialize

        private readonly IConfiguration _configuration;

        public JwtAppService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion Initialize

        #region Implements

        public JwtAuthorizationDto CreateJwt(JwtUserDto jwtUserDto)
        {
            try
            {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]!));

                DateTime authTime = DateTime.Now;
                DateTime expireTime = authTime.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));

                // 将用户信息添加到Claim中
                List<Claim> claims = new List<Claim>()
                {
                    new Claim("userId",jwtUserDto.UserId.ToString()),
                    new Claim("userName",jwtUserDto.UserName),
                    new Claim("roleId",jwtUserDto.RoleId.ToString()),
                };
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return default;
            }
        }

        #endregion Implements
    }
}