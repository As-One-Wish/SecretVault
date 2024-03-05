using FreeSql.DataAnnotations;
using Info.Storage.Infra.Entity.ModuleUserManagement.Enums;
using Newtonsoft.Json;

namespace Info.Storage.Infra.Repository.Databases.Entities
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "user")]  // MemberSerialization.OptIn -> 表示只有被显式标记为'JsonProperty'的成员才会被包括在序列化中
    public class AppUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [JsonProperty, Column(Name = "user_id", IsPrimary = true)]
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty, Column(Name = "user_name", DbType = "varchar(60)")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 用户账号
        /// </summary>
        [JsonProperty, Column(Name = "user_account", DbType = "varchar(60)")]
        public string UserAccount { get; set; } = string.Empty;

        /// <summary>
        /// 用户密码
        /// </summary>
        [JsonProperty, Column(Name = "user_pwd", DbType = "varchar(60)")]
        public string UserPwd { get; set; } = string.Empty;

        /// <summary>
        /// 用户电话
        /// </summary>
        [JsonProperty, Column(Name = "user_phone", DbType = "varchar(60)")]
        public string UserPhone { get; set; } = string.Empty;

        /// <summary>
        /// 用户头像
        /// </summary>
        [JsonProperty, Column(Name = "user_avatar", DbType = "text")]
        public string UserAvatar { get; set; } = string.Empty;

        /// <summary>
        /// 用户性别
        /// </summary>
        [JsonProperty, Column(Name = "user_gender", MapType = typeof(short))]
        public GenderEnum UserGender { get; set; } = GenderEnum.U;

        /// <summary>
        /// 用户角色ID
        /// </summary>
        [JsonProperty, Column(Name = "role_id", DbType = "int8")]
        public long RoleId { get; set; }

        /// <summary>
        /// 是否删除标记
        /// </summary>
        [JsonProperty, Column(Name = "deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty, Column(Name = "update_time", ServerTime = DateTimeKind.Local)] // 写入数据库时使用本地时间
        public DateTime? UpdateTime { get; set; }
    }
}