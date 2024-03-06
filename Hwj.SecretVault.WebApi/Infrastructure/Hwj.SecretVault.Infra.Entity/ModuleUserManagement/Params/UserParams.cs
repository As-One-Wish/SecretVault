using Hwj.SecretVault.Infra.Entity.Shared.Params;

namespace Hwj.SecretVault.Infra.Entity.ModuleUserManagement.Params
{
    /// <summary>
    /// 用户查询参数
    /// </summary>
    public record QueryUserParam : BaseParam
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }
        /// <summary>
        /// 是否显示头像
        /// </summary>
        public bool ShowAvatar { get; set; } = false;
        /// <summary>
        /// 查询文本
        /// </summary>
        public string SearchText { get; set; }
    }

    /// <summary>
    /// 用户删除参数
    /// </summary>
    public record DeleteUserParam
    {
        public long? UserId { get; set; }
        public long[] UserIds { get; set; }
    }
}