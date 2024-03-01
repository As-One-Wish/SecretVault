using Info.Storage.Infa.Entity.ModuleInfoManagement.Enums;
using Info.Storage.Infa.Entity.Shared.Params;

namespace Info.Storage.Infa.Entity.ModuleInfoManagement.Params
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
        /// 信息类型
        /// </summary>
        public InfoTypeEnum? InfoType { get; set; }
        /// <summary>
        /// 查询文本
        /// </summary>
        public string SearchText { get; set; }
    }
}