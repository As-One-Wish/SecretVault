using Hwj.SecretVault.Infra.Entity.ModuleInfoManagement.Enums;

namespace Hwj.SecretVault.Infra.Entity.ModuleInfoManagement.Dtos
{
    public record struct InfoDto
    {
        public InfoDto()
        {
            InfoId = -1;
            InfoName = string.Empty;
            Account = null;
            InfoType = InfoTypeEnum.Unknown;
            Remark = string.Empty;
            UserId = -1;
        }
        /// <summary>
        /// 信息Id
        /// </summary>
        public long InfoId { get; set; }
        /// <summary>
        /// 信息名称
        /// </summary>
        public string InfoName { get; set; }
        /// <summary>
        /// 账号--类型为密码时
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 信息类型
        /// </summary>
        public InfoTypeEnum InfoType { get; set; }
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 所属用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}