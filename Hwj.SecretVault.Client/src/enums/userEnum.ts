/**
 * @description: 性别枚举
 */
export enum GenderEnum {
  U = 0, // 未知
  M = 1, // 男
  F = 2 // 女
}

export const GenderEnumMap = new Map([
	[GenderEnum.U, { label: '未知', color: '#708090' }],
	[GenderEnum.M, { label: '男', color: '#6fa8dc' }],
	[GenderEnum.F, { label: '女', color: '#ffb6c1' }]
])