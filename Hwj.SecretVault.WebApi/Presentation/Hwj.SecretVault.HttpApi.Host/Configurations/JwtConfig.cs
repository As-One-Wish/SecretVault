using Info.Storage.HttpApi.Host.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Info.Storage.HttpApi.Host.Configurations
{
    /// <summary>
    /// Jwt配置
    /// </summary>
    public static class JwtConfig
    {
        /// <summary>
        /// Jwt注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            // 从配置中获取对应信息
            string? issuer = configuration["Jwt:Issuer"]; // 颁发者
            string? audience = configuration["Jwt:Audience"]; // 受众
            string? expire = configuration["Jwt:ExpireMinutes"]; // 过期时间
            string? securityKey = configuration["Jwt:SecurityKey"]; // 安全密钥
            // 配置信息转换
            TimeSpan expiration = TimeSpan.FromMinutes(Convert.ToDouble(expire));
            SecurityKey key = new SymmetricSecurityKey(securityKey == null ? new byte[0] : Encoding.UTF8.GetBytes(securityKey));
            // Jwt相关配置
            services.AddAuthorization(options => // 1.配置授权策略
            {
                options.AddPolicy("Policy.Default", policy => policy.Requirements.Add(new PolicyRequirement("Default")));
                options.AddPolicy("Policy.Admin", policy => policy.Requirements.Add(new PolicyRequirement("Admin")));
            }).AddAuthentication(s =>  // 2.配置身份验证
            {
                // 在身份验证成功后，默认使用的身份验证方案
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // 在请求处理过程中使用的默认身份验证方案
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                // 在发生身份验证挑战时使用的默认身份验证方案
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(s => // 3.配置Jwt Bearer (Token的鉴权逻辑)
            {
                // 配置 Jwt Bearer 的验证参数
                s.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer, // 颁发者的有效值
                    ValidAudience = audience, // 受众的有效值
                    IssuerSigningKey = key, // 用于验证签名的密钥
                    ClockSkew = expiration, //允许的时钟偏差
                    ValidateLifetime = true, // 是否验证令牌的生命周期
                    RequireExpirationTime = true, // 是否要求令牌包含有效期
                    ValidateIssuer = true, // 是否验证Issuer
                    ValidateAudience = true, // 是否验证Audience
                    ValidateIssuerSigningKey = true // 是否验证SecurityKey
                };
                // 配置 Jwt Bearer 的事件处理器
                s.Events = new JwtBearerEvents
                {
                    // 收到消息时的触发的事件
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Method == "GET" && !string.IsNullOrWhiteSpace(context.Request.Query["token"]))
                            context.Token = context.Request.Query["token"];
                        return Task.CompletedTask;
                    },
                    // 当身份验证失败时触发的事件
                    OnAuthenticationFailed = context =>
                    {
                        // 令牌过期异常
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.StatusCode = 200;
                            context.Response.Headers.Append("WWW-AUthenticate", "Bearer error=\"token_refreshed\"");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSingleton<IAuthorizationHandler, PolicyHandler>();
        }
    }
}