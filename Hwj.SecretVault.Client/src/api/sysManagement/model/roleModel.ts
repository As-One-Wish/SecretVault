import { BaseParam } from '@/api/common/model/baseModel'

/**
 * @description 角色Dto
 */
export type RoleDto = {
  roleId: number,
  roleName: string,
  roleCnName: string,
  remark: string,
  updateTime: string
}
/**
 * @description 角色查询参数
 */
export type QueryRoleParam = {
  roleId?: number,
  searchText?: string
} & BaseParam
/**
 * @description 角色删除参数
 */
export type DeleteRoleParam = {
  roleId?: number,
  roleIds?: number[]
}