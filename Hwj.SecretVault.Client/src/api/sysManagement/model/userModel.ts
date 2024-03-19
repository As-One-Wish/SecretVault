import { BaseParam, BaseResult } from '@/api/common/model/baseModel'
import { GenderEnum } from '@/enums/userEnum'
import { RoleDto } from './roleModel'

/**
 * @description: 用户登录参数
 */
export type JwtLoginParam = {
  account: string,
  password: string
}
/**
 * @description: 用户登录成功返回Dto
 */
export type JwtAuthorizationDto = {
  token: string,
  authTime: number,
  expireTime: number
}

export type JwtAuthorizationDtoResult = BaseResult<JwtAuthorizationDto>
/**
 * @description: 删除用户的请求参数
 */
export type DeleteUserParam = {
  userId?: number,
  userIds?: number[]
}
/**
 * @description: 查询用户的请求参数
 */
export type QueryUserParam = {
  userId?: number,
  showAvatar?: boolean,
  searchText?: string
} & BaseParam
/**
 * @description: 用户Dto
 */
export type UserDto = {
  userId: number,
  userName: string,
  account: string,
  password: string,
  avatar: string,
  phone: string,
  gender: GenderEnum,
  roleId: number,
  roleName?: string,
  updateTime: Date,
  homePath?: string
}
/**
 * @description 用户登录相关Dto
 */
export type UserLoginRelatedDto = {
  user?: UserDto,
  role?: RoleDto
}