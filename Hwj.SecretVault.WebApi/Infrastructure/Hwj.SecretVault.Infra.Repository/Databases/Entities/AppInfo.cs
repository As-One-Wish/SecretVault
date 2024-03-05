using FreeSql.DataAnnotations;
using Info.Storage.Infra.Entity.ModuleInfoManagement.Enums;
using Newtonsoft.Json;

namespace Info.Storage.Infra.Repository.Databases.Entities
{
    /// <summary>
    /// 信息内容表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "info")]
    public class AppInfo
    {
        /// <summary>
        /// 信息ID
        /// </summary>
        [JsonProperty, Column(Name = "info_id", IsPrimary = true)]
        public long InfoId { get; set; }

        /// <summary>
        /// 信息名称
        /// </summary>
        [JsonProperty, Column(Name = "info_name", DbType = "varchar(60)")]
        public string InfoName { get; set; } = string.Empty;

        /// <summary>
        /// 账号-当类型为密码时
        /// </summary>
        [JsonProperty, Column(Name = "info_account", DbType = "varchar(60)")]
        public string InfoAccount { get; set; } = string.Empty;

        /// <summary>
        /// 信息内容
        /// </summary>
        [JsonProperty, Column(Name = "info_content", DbType = "text")]
        public string InfoContent { get; set; } = string.Empty;

        /// <summary>
        /// 信息类型
        /// </summary>
        [JsonProperty, Column(Name = "info_type", MapType = typeof(short))]
        public InfoTypeEnum InfoType { get; set; } = InfoTypeEnum.Unknown;

        /// <summary>
        /// 信息备注
        /// </summary>
        [JsonProperty, Column(Name = "info_remark", DbType = "text")]
        public string InfoRemark { get; set; } = string.Empty;

        /// <summary>
        /// 所属用户ID
        /// </summary>
        [JsonProperty, Column(Name = "user_id")]
        public long UserId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [JsonProperty, Column(Name = "deleted")]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty, Column(Name = "update_time", ServerTime = DateTimeKind.Local)]
        public DateTime UpdateTime { get; set; }
    }
}