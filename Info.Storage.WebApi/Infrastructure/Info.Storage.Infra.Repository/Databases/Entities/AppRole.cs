using FreeSql.DataAnnotations;
using Newtonsoft.Json;

namespace Info.Storage.Infra.Repository.Databases.Entities
{
    /// <summary>
    /// 用户角色信息表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "role")]
    public class AppRole
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [JsonProperty, Column(Name = "role_id", IsPrimary = true)]
        public long RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [JsonProperty, Column(Name = "role_name", DbType = "varchar(60)")]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色中文名
        /// </summary>
        [JsonProperty, Column(Name = "role_cn_name", DbType = "varchar(60)")]
        public string RoleCnName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty, Column(Name = "role_remark", DbType = "varchar(60)")]
        public string RoleRemark { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty, Column(Name = "update_time", ServerTime = DateTimeKind.Local)] // 写入数据库时使用本地时间
        public DateTime UpdateTime { get; set; }
    }
}