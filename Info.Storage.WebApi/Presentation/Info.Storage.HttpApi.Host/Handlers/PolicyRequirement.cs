using Microsoft.AspNetCore.Authorization;

namespace Info.Storage.HttpApi.Host.Handlers
{
    /// <summary>
    /// 策略声明
    /// </summary>
    public class PolicyRequirement : IAuthorizationRequirement
    {
        public string RequiredRole { get; }

        public PolicyRequirement(string requiredRole)
        {
            RequiredRole = requiredRole;
        }
    }
}