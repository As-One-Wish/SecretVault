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
                // 签发一个加密后的用户信息凭证，用来标识用户身份
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims), // 创建声明信息
                    Issuer = _configuration["Jwt:Issuer"], // Jwt token 的签发者
                    Audience = _configuration["Jwt:Audience"], // Jwt token 的接受者
                    Expires = expireTime, // 过期时间
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256), // 创建 token
                };

                var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
                // 存储token信息
                var jwt = new JwtAuthorizationDto
                {
                    Token = jwtSecurityTokenHandler.WriteToken(token),
                    AuthTime = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                    ExpireTime = new DateTimeOffset(expireTime).ToUnixTimeSeconds(),
                };
                return jwt;
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