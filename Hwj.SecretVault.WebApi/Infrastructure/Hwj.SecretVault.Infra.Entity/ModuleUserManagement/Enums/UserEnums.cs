using System.ComponentModel;

namespace Info.Storage.Infra.Entity.ModuleUserManagement.Enums
{
    /// <summary>
    /// 性别枚举
    /// </summary>
    public enum GenderEnum
    {
        [Description("未知")]
        U = 0,

        [Description("男")]
        M = 1,

        [Description("女")]
        F = 2
    }
}