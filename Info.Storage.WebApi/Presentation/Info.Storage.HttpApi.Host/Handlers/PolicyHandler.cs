using Info.Storage.HttpApi.Host.Handler;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;

namespace Info.Storage.HttpApi.Host.Handlers
{
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

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            // 获取当前资源
            HttpContext? httpContext = context.Resource as HttpContext;
            // 获取默认的身份验证方案
            AuthenticationScheme? defaultAuthenicate = await schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenicate != null && httpContext != null)
            {
                // 验证签发的用户信息
                var result = await httpContext.AuthenticateAsync(defaultAuthenicate.Name);
                if (result.Succeeded)
                {
                    bool isSucceedRolesAuthorization = true;
                    bool isSucceedClaimsAuthorization = true;

                    foreach (var itemRequirement in context.Requirements)
                    {
                        // 角色授权要求
                        if (itemRequirement.GetType().Name == "RolesAuthorizationRequirement")
                        {
                            // 获取用户角色
                            string? roleRequire = result.Principal.Claims?.FirstOrDefault(d => d.Type == ClaimTypes.Role)?.Value;

                            isSucceedRolesAuthorization = ((RolesAuthorizationRequirement)itemRequirement).AllowedRoles.Contains(roleRequire);
                        }
                        // 声明授权要求
                        if (itemRequirement.GetType().Name == "ClaimsAuthorizationRequirement")
                        {
                            // 强转为声明授权要求
                            ClaimsAuthorizationRequirement? itemClaimsRequirement = itemRequirement as ClaimsAuthorizationRequirement;
                            if (itemClaimsRequirement != null)
                            {
                                List<string>? claimsRequire = result.Principal.Claims?.Where(d => d.Type == itemClaimsRequirement.ClaimType).Select(d => d.Value).ToList();
                                if (claimsRequire == null || claimsRequire.Count() == 0)
                                    isSucceedClaimsAuthorization = false;
                                else
                                {
                                    if (itemClaimsRequirement.AllowedValues != null)
                                        isSucceedClaimsAuthorization = claimsRequire.Any(d => itemClaimsRequirement.AllowedValues.Contains(d));
                                }
                            }
                        }
                    }
                    if (isSucceedRolesAuthorization && isSucceedClaimsAuthorization)
                        context.Succeed(requirement);
                    else
                    {
                        if (!httpContext.Response.Headers.ContainsKey("WWW-Authenticate"))
                            httpContext.Response.Headers.Append("WWW-Authenticate", "Bearer error=\"invalid_permission\"");
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