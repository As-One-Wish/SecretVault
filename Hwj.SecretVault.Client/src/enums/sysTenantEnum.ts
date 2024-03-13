/*
 * @Author: arcchen
 * @Description:
 * @Date: 2022-11-04 09:59:05
 * @LastEditTime: 2023-06-02 11:11:56
 * @FilePath: \CastcZhy.Tmp.WebClient.VbenAdmin\src\enums\sysTenantEnum.ts
 */
/**
 * @description: 系统租户类型枚举
 *  /// <summary>
    /// 租户类型
    /// </summary>
    public enum TenantTypeEnum
    {
        [Display(Name = "未知")]
        Unknown = 0,
        [Display(Name = "系统默认租户")]
        Default = 1,
        [Display(Name = "自定义租户")]
        Custom = 2,
    }
 * @event:
 * @return {*}
 */
export enum SysTenantTypeEnum {
  Unknown = 0,
  Default = 1,
  Custom = 2
}
/**
 * @description: 授系统租户类型枚举属性Map
 * @event:
 * @return {*}
 */
export const SysTenantTypeEnumMap = new Map([
	[SysTenantTypeEnum.Unknown, { label: '未知', color: '#708090' }],
	[SysTenantTypeEnum.Default, { label: '系统默认租户', color: '#0926f2' }],
	[SysTenantTypeEnum.Custom, { label: '自定义租户', color: '#008000' }]
])
