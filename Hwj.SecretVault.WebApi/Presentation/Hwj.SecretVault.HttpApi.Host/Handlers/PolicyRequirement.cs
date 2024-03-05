using Microsoft.AspNetCore.Authorization;

namespace Info.Storage.HttpApi.Host.Handlers
{
    /// <summary>
    /// 策略声明
    /// </summary>
    public class PolicyRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 要求的角色
        /// </summary>
        public string RequiredRole { get; }

        /// <summary>
        /// 策略要求构造函数
        /// </summary>
        /// <param name="requiredRole"></param>
        public PolicyRequirement(string requiredRole)
        {
            RequiredRole = requiredRole;
        }
    }
}