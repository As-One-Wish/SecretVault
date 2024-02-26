using Info.Storage.Infa.Entity.ModuleUserManagement.Enums;

namespace Info.Storage.Infa.Entity.ModuleUserManagement.Dtos
{
    public record struct UserDto
    {
        public UserDto()
        {
            UserId = -1;
            UserName = string.Empty;
            Account = string.Empty;
            Gender = GenderEnum.U;
            Phone = string.Empty;
            Avatar = string.Empty;
            RoleId = null;
            RoleName = null;
            Password = string.Empty;
            UpdateTime = DateTime.Now;
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 用户电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户性别
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 用户角色ID
        /// </summary>
        public long? RoleId { get; set; }
        public string RoleName { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}