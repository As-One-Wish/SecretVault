/*
 * @Author: arcchen
 * @Description:
 * @Date: 2022-11-16 13:42:38
 * @LastEditTime: 2022-11-16 15:00:35
 * @FilePath: \adf_app_webclient\src\enums\deptEnum.ts
 */
export enum DeptLevelEnum {
  Unknown = 0,
  Level1 = 1,
  Level2 = 2,
  Level3 = 3,
  Level4 = 4,
  Level5 = 5,
}
export const DeptLevelEnumMap = new Map([
	[DeptLevelEnum.Unknown, { label: '未知', color: '#000' }],
	[DeptLevelEnum.Level1, { label: '一级', color: '#000' }],
	[DeptLevelEnum.Level2, { label: '二级', color: '#000' }],
	[DeptLevelEnum.Level3, { label: '三级', color: '#000' }],
	[DeptLevelEnum.Level4, { label: '四级', color: '#000' }],
	[DeptLevelEnum.Level5, { label: '五级', color: '#000' }]
])
