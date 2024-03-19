using Hwj.Aow.Utils.CommonHelper.Extensions;
using Hwj.Aow.Utils.CommonHelper.Helpers;
using Hwj.SecretVault.Infra.Entity.ModuleAuthorization.Dtos;
using Hwj.SecretVault.Infra.Entity.Shared.Attributes;
using Hwj.SecretVault.Infra.Entity.Shared.Constants;
using Hwj.SecretVault.Infra.Entity.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hwj.SecretVault.Application.ModuleAuthorization
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

        /// <summary>
        /// 刷新Token-延续
        /// </summary>
        /// <param name="oldToken"></param>
        /// <param name="oldExpireTime"></param>
        /// <returns></returns>
        BaseResult<JwtAuthorizationDto?> RefreshJwt(string oldToken, long oldExpireTime);

        /// <summary>
        /// 解析Token
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        BaseResult<(long, JwtAuthorizationDto?)> DecodeJwt(string jwtToken);
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
                // 创建 JwtSecurityTokenHandler 实例，用于处理 JWT 操作
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]!));

                DateTime authTime = DateTime.Now;
                DateTime expireTime = authTime.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));

                // 将用户信息添加到Claim(声明)中
                List<Claim> claims = new List<Claim>()
                {
                    new Claim("userId",jwtUserDto.UserId.ToString()),
                    new Claim("userName",jwtUserDto.UserName),
                    new Claim("roleId",jwtUserDto.RoleId.ToString()),
                    new Claim("account",jwtUserDto.Account),
                    new Claim("roleName", jwtUserDto.RoleName)
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

        public BaseResult<JwtAuthorizationDto?> RefreshJwt(string oldToken, long oldExpireTime)
        {
            try
            {
                BaseResult<JwtAuthorizationDto?> result = new BaseResult<JwtAuthorizationDto?>();
                if (!string.IsNullOrWhiteSpace(oldToken))
                {
                    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jst = tokenHandler.ReadJwtToken(oldToken.Replace("Bearer ", ""));

                    // 解析oldExpireTime
                    Claim expireTimeClaim = jst.Claims.Where(claim => claim.Type == "exp").First();
                    // 验证
                    if (Convert.ToInt64(expireTimeClaim.Value) == oldExpireTime)
                    {
                        DateTime authTime = DateTime.Now;
                        DateTime expireTime = authTime.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));
                        List<Claim> claims = jst.Claims.ToList();
                        claims.RemoveAll(claim => claim.Type == ClaimTypes.Expiration || claim.Type == "aud" || claim.Type == "exp");
                        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(claims),
                            Issuer = _configuration["Jwt:Issuer"],
                            Audience = _configuration["Jwt:Audience"],
                            Expires = expireTime,
                            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                        };

                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        result.IsSuccess = true;
                        result.Data = new JwtAuthorizationDto
                        {
                            Token = tokenHandler.WriteToken(token),
                            AuthTime = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                            ExpireTime = new DateTimeOffset(expireTime).ToUnixTimeSeconds()
                        };
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Data = null;
                        result.Message = "token验证码无效！";
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Data = null;
                    result.Message = "token不能为空！";
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new BaseResult<JwtAuthorizationDto?>(false, null, ex.Message);
            }
        }

        public BaseResult<(long, JwtAuthorizationDto?)> DecodeJwt(string jwtToken)
        {
            try
            {
                BaseResult<(long, JwtAuthorizationDto?)> result = new BaseResult<(long, JwtAuthorizationDto?)>();
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jst = tokenHandler.ReadJwtToken(jwtToken.Replace("Bearer ", ""));
                // 解析UserID
                Claim userIdClaim = jst.Claims.Where(a => a.Type == "userId").First();
                Claim authTimeClaim = jst.Claims.Where(a => a.Type == "iat").First();
                Claim expireTimeClaim = jst.Claims.Where(a => a.Type == "exp").First();

                if (userIdClaim != null && authTimeClaim != null && expireTimeClaim != null)
                {
                    bool isExpired = Convert.ToInt64(expireTimeClaim.Value) < DateTime.Now.ToUnixTime();
                    if (isExpired)
                        result = new BaseResult<(long, JwtAuthorizationDto?)>(false, (-1, null), Msg.AuthExpire);
                    else
                    {
                        long userId = Convert.ToInt64(userIdClaim.Value);
                        JwtAuthorizationDto jwtAuthorizationDto = new JwtAuthorizationDto();
                        jwtAuthorizationDto.AuthTime = Convert.ToInt64(authTimeClaim.Value);
                        jwtAuthorizationDto.ExpireTime = Convert.ToInt64(expireTimeClaim.Value);
                        jwtAuthorizationDto.Token = jwtToken.Replace("Bearer", "");

                        result = new BaseResult<(long, JwtAuthorizationDto?)>(true, (userId, jwtAuthorizationDto));
                    }
                }
                else
                    result = new BaseResult<(long, JwtAuthorizationDto?)>(false, (-1, null), Msg.Exception);
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return new BaseResult<(long, JwtAuthorizationDto?)>(false, (-1, null), ex.Message);
            }
        }

        #endregion Implements
    }
}