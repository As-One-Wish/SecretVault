/*
 * @Author: arcchen
 * @Description:
 * @Date: 2022-11-03 11:08:18
 * @LastEditTime: 2022-12-04 20:54:06
 * @FilePath: \CastcZhy.Tmp.WebClient.VbenAdmin\src\enums\userEnum.ts
 */
/**
 * @description: 性别枚举
 * @event:
 * @return {*}
 */
export enum GenderEnum {
  U = 0, //未知
  M = 1, //男
  F = 2, //女
}
/**
 * @description: 授权角色枚举属性Map
 * @event:
 * @return {*}
 */
export const GenderEnumMap = new Map([
	[GenderEnum.U, { label: '未知', color: '#708090' }],
	[GenderEnum.M, { label: '男', color: '#006400' }],
	[GenderEnum.F, { label: '女', color: '#008000' }]
])
