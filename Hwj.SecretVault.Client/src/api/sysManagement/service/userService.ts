// import { useGlobSetting } from '@/hooks/setting'
import { DeleteUserParam, JwtAuthorizationDtoResult, JwtLoginParam, QueryUserParam, UserDto } from '../model/userModel'
import { ErrorMessageMode } from '#/axios'
import { AesEncryption, EncryptionParams } from '@/utils/cipher'
import { cacheCipher } from '@/settings/encryptionSetting'
import { defHttp } from '@/utils/http/axios'
import { BaseResult } from '@/api/common/model/baseModel'

// Aes加密
const encryption = new AesEncryption(cacheCipher as Partial<EncryptionParams>)

// const globSetting = useGlobSetting()
enum Api {
  CreateToken = '/Authorization/CreateToken',
  RefreshToken = '/Authorization/RefreshToken',

  GetUser = '/User/GetUserById',
  GetUsers = '/User/GetUsers',
  AddUser = '/User/AddUser',
  DelUser = '/User/DelUser',
  UpdateUser = '/User/UpdateUser',
  IsUserAccountExist = '/User/IsUserAccountExist',
	GetUserLoginRelated = '/User/GetUserLoginRelated'
}
/**
 * @description 获取Token
 * @param params 
 * @param mode 
 * @returns 
 */
export function createToken(params: JwtLoginParam, mode: ErrorMessageMode = 'modal') {
	params.password = encryption.encryptByAES(params.password)
	return defHttp.post<JwtAuthorizationDtoResult>(
		{
			url: Api.CreateToken,
			params
		},
		{
			errorMessageMode: mode,
			withToken: false
		}
	)
}
/**
 * @description 刷新Token
 * @param params 刷新码
 * @param mode 
 * @returns 
 */
export function refreshToken(params: number, mode: ErrorMessageMode = 'message') {
	return defHttp.post<JwtAuthorizationDtoResult>(
		{
			url: Api.RefreshToken,
			params
		},
		{
			errorMessageMode: mode
		}
	)
}
/**
 * @description 查询单一用户ById
 * @param params 
 * @returns
 */
export function getUser(params?: number) {
	return defHttp.get<BaseResult<UserDto>>(
		{ url: Api.GetUser, params },
		{ errorMessageMode: 'message' }
	)
}
/**
 * @description 查询用户
 * @param params
 * @returns
 */
export function getUsers(params?: QueryUserParam){
	return defHttp.get<BaseResult<UserDto[]>>(
		{url: Api.GetUsers, params},
		{errorMessageMode: 'message'}
	)
}
/**
 * @description 添加用户
 * @param params 
 * @param responseData 
 * @returns
 */
export function addUser(params?: UserDto, responseData = false){
	return defHttp.post<BaseResult<UserDto>>(
		{url: `${Api.AddUser}?responseData=${responseData}`,params},
		{errorMessageMode: 'message'}
	)
}
/**
 * @description 删除用户
 * @param params 
 * @returns
 */
export function delUser(params?: DeleteUserParam){
	return defHttp.delete<BaseResult>(
		{url: Api.DelUser, params},
		{errorMessageMode: 'message'}
	)
}
/**
 * @description 更新用户信息
 * @param params 
 * @param responseData 
 * @returns 
 */
export function updateUser(params: UserDto, responseData = false){
	return defHttp.post<BaseResult<UserDto>>(
		{url: `${Api.UpdateUser}?responseData=${responseData}`,params},
		{errorMessageMode: 'message'}
	)
}
/**
 * @description 判断账号是否存在
 * @param params 
 * @returns 
 */
export function isUserAccountExist(params: string){
	return defHttp.post<BaseResult>(
		// 配置header,设置ocelot的限流白名单
		{url: Api.IsUserAccountExist,params: {params},headers: {gatewayHeader: 'host-web-client'}}
	)
}
/**
 * @description 获取用户登录相关信息
 * @returns 
 */
export function getUserLoginRelated(){
	return defHttp.get<BaseResult<UserDto>>(
		{url: Api.GetUserLoginRelated},
		{errorMessageMode: 'none'}
	)
}