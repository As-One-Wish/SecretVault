using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Info.Storage.HttpApi.Host.Handlers
{
    /// <summary>
    /// 授权 Handler
    /// </summary>
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        /// <summary>
        /// 授权方式(cookie, bearer, oauth, openid)
        /// </summary>
        public IAuthenticationSchemeProvider schemes { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="schemes"></param>
        public PolicyHandler(IAuthenticationSchemeProvider schemes)
        {
            this.schemes = schemes;
        }

        /// <summary>
        /// Requirement处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            HttpContext? httpContext = context.Resource as HttpContext;
            var defaultAuthenticate = await schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null && httpContext != null)
            {
                // 验证签发的用户信息
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                if (result.Succeeded)
                {
                    string? roleNameClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "roleName")?.Value;
                    if (!string.IsNullOrWhiteSpace(roleNameClaim) && roleNameClaim == requirement.RequiredRole || requirement.RequiredRole == "Default")
                        context.Succeed(requirement);
                    else
                    {
                        if (!httpContext.Response.Headers.ContainsKey("WWW-Authenticate"))
                        {
                            httpContext.Response.Headers.Append("WWW-Authenticate", "Bearer error=\"invalid_permission\"");
                        }
                        context.Fail();
                    }
                }
                else
                {
                    httpContext.Response.Headers.Append("WWW-Authenticate", "Bearer error=\"invalid_token\"");
                    context.Fail();
                }
            }
        }
    }
}