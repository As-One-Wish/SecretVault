using System.ComponentModel;

namespace Info.Storage.Infa.Entity.ModuleInfoManagement.Enums
{
    /// <summary>
    /// 信息类型枚举
    /// </summary>
    public enum InfoTypeEnum
    {
        [Description("未知")]
        Unknown = 0,

        [Description("普通")]
        Normal = 1,

        [Description("密码")]
        Password = 2
    }
}