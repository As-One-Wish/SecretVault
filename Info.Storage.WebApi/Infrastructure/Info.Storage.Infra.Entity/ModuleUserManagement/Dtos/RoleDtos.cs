namespace Info.Storage.Infra.Entity.ModuleUserManagement.Dtos
{
    public record struct RoleDto
    {
        public RoleDto()
        {
            RoleId = -1;
            RoleName = null;
            RoleCnName = null;
            Remark = null;
            UpdateTime = DateTime.Now;
        }
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色中文名称
        /// </summary>
        public string RoleCnName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}