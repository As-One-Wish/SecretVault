/*
 * @Author: arcchen
 * @Description:
 * @Date: 2022-12-07 23:20:32
 * @LastEditTime: 2022-12-17 19:16:23
 * @FilePath: \CastcZhy.Tmp.WebClient.VbenAdmin\src\enums\sysSettingEnum.ts
 */
/**
 * @description: 系统设置之全局配置类型枚举
 * @event:
 * @return {*}
 */
export enum SysSettingGlobalConfigTypeEnum {
  AuthPageSchm = 0,
  AuthComponentSchm = 1,
  AuthApiModuleSchm = 2,
  MapStyleSchm = 3,
}
/**
 * @description: 系统设置之全局配置类型枚举属性Map
 * @event:
 * @return {*}
 */
export const SysSettingGlobalConfigTypeEnumMap = new Map([
	[SysSettingGlobalConfigTypeEnum.AuthPageSchm, { label: '页面授权默认方案', color: '#708090' }],
	[
		SysSettingGlobalConfigTypeEnum.AuthComponentSchm,
		{ label: '前端组件默认方案', color: '#006400' }
	],
	[
		SysSettingGlobalConfigTypeEnum.AuthApiModuleSchm,
		{ label: 'API模块授权默认方案', color: '#008000' }
	],
	[SysSettingGlobalConfigTypeEnum.MapStyleSchm, { label: '地图样式默认方案', color: '#228B22' }]
])
