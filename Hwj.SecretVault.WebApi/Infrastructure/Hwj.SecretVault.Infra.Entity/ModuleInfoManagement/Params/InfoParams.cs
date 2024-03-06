using Hwj.SecretVault.Infra.Entity.ModuleInfoManagement.Enums;
using Hwj.SecretVault.Infra.Entity.Shared.Params;

namespace Hwj.SecretVault.Infra.Entity.ModuleInfoManagement.Params
{
    /// <summary>
    /// 信息删除参数
    /// </summary>
    public record DeleteInfoParam
    {
        public long? InfoId { get; set; }
        public long[] InfoIds { get; set; }
    }
    /// <summary>
    /// 信息查询参数
    /// </summary>
    public record QueryInfoParam : BaseParam
    {
        /// <summary>
        /// 信息Id
        /// </summary>
        public long? InfoId { get; set; }
        /// <summary>
        /// 所属用户Id
        /// </summary>
        public long? UserId { get; set; }
        /// <summary>
        /// 信息类型
        /// </summary>
        public InfoTypeEnum? InfoType { get; set; }
        /// <summary>
        /// 查询文本
        /// </summary>
        public string SearchText { get; set; }
    }
}