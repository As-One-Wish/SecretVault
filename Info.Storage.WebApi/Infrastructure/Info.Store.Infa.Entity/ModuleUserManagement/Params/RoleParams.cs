using Info.Storage.Infa.Entity.Shared.Params;

namespace Info.Storage.Infa.Entity.ModuleUserManagement.Params
{
    /// <summary>
    /// 角色删除参数
    /// </summary>
    public record DeleteRoleParam
    {
        public long? RoleId { get; set; }
        public long[] RoleIds { get; set; }
    }
    /// <summary>
    /// 角色查询参数
    /// </summary>
    public record QueryRoleParam : BaseParam
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long? RoleId { get; set; }
        /// <summary>
        /// 查询文本
        /// </summary>
        public string SearchText { get; set; }
    }
}