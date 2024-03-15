import type { ErrorMessageMode } from '#/axios'
import { defineStore } from 'pinia'
import { store } from '@/store'
import { RoleEnum } from '@/enums/roleEnum'
import { PageEnum } from '@/enums/pageEnum'
import { ROLE_KEY, TOKEN_EXPIRE_TIME_KEY, TOKEN_KEY, USER_KEY } from '@/enums/cacheEnum'
import { getAuthCache, setAuthCache } from '@/utils/auth'
import { doLogout, getUserInfo, loginApi } from '@/api/sys/user'
import { t, useI18n } from '@/hooks/web/useI18n'
import { useMessage } from '@/hooks/web/useMessage'
import { router } from '@/router'
import { usePermissionStore } from '@/store/modules/permission'
import { RouteRecordRaw } from 'vue-router'
import { PAGE_NOT_FOUND_ROUTE } from '@/router/routes/basic'
import { isArray } from '@/utils/is'
import { h } from 'vue'
import { JwtLoginParam, UserDto } from '@/api/sysManagement/model/userModel'
import { RoleDto } from '@/api/sysManagement/model/roleModel'
import { createToken, getUser } from '@/api/sysManagement/service/userService'

interface UserState {
  user: Nullable<UserDto>
  token?: string
  role: Nullable<RoleDto>
  sessionTimeout?: boolean
  expireTokenTime: number
}

export const useUserStore = defineStore({
	id: 'app-user',
	state: (): UserState => ({
		// user 用户信息
		user: null,
		// token
		token: undefined,
		// role 角色信息
		role: null,
		// Whether the login expired
		sessionTimeout: false,
		// token expire time Token过期时间
		expireTokenTime: 0
	}),
	getters: {
		getUser(): UserDto {
			return this.user || getAuthCache<UserDto>(USER_KEY) || {}
		},
		getToken(): string {
			return this.token || getAuthCache<string>(TOKEN_KEY)
		},
		getRole(): RoleDto {
			return this.role || getAuthCache<RoleDto>(ROLE_KEY) || {}
		},
		getSessionTimeout(): boolean {
			return !!this.sessionTimeout
		},
		getExpireTokenTime(): number {
			return this.expireTokenTime == 0 ? getAuthCache<number>(TOKEN_EXPIRE_TIME_KEY) : this.expireTokenTime
		}
	},
	actions: {
		setToken(info: string | undefined) {
			this.token = info ? info : '' // for null or undefined value
			setAuthCache(TOKEN_KEY, info)
		},
		setRole(role: RoleDto | undefined | null) {
			if(role){
				this.role = role
				setAuthCache(ROLE_KEY, role)
			}
		},
		setUser(user: UserDto|undefined | null) {
			if(user){
				this.user = user
				setAuthCache(USER_KEY, user)
			}
		},
		setSessionTimeout(flag: boolean) {
			this.sessionTimeout = flag
		},
		setExpireTokenTime(time: number |undefined){
			this.expireTokenTime = time ? time : 0
			setAuthCache(TOKEN_EXPIRE_TIME_KEY, time)
		},
		resetState() {
			this.user = null
			this.token = ''
			this.role = null
			this.sessionTimeout = false
		},
		/**
     * @description: login
     */
		async login(
			params: JwtLoginParam & {
        goHome?: boolean
        mode?: ErrorMessageMode
      }
		): Promise<UserDto | null> {
			try {
				const { goHome = true, mode, ...jwtLoginParam } = params
				const result = await createToken(jwtLoginParam, mode)
				const { data } = result

				// save token
				this.setToken(data?.token)
				this.setExpireTokenTime(data?.expireTime)
				return this.afterLoginAction(goHome)
			} catch (error) {
				return Promise.reject(error)
			}
		},
		async afterLoginAction(goHome?: boolean): Promise<UserDto | null> {
			if (!this.getToken) return null
			// get user info
			const userInfo = await this.getUserInfoAction()

			const sessionTimeout = this.sessionTimeout
			if (sessionTimeout) {
				this.setSessionTimeout(false)
			} else {
				const permissionStore = usePermissionStore()
				if (!permissionStore.isDynamicAddedRoute) {
					const routes = await permissionStore.buildRoutesAction()
					routes.forEach((route) => {
						router.addRoute(route as unknown as RouteRecordRaw)
					})
					router.addRoute(PAGE_NOT_FOUND_ROUTE as unknown as RouteRecordRaw)
					permissionStore.setDynamicAddedRoute(true)
				}
				goHome && (await router.replace(userInfo?.homePath || PageEnum.BASE_HOME))
			}
			return userInfo
		},
		async getUserAction(): Promise<UserDto | null> {
			if (!this.getToken) return null
			const userInfo = await getUser()
			const { roles = [] } = userInfo
			if (isArray(roles)) {
				const roleList = roles.map((item) => item.value) as RoleEnum[]
				this.setRoleList(roleList)
			} else {
				userInfo.roles = []
				this.setRoleList([])
			}
			this.setUserInfo(userInfo)
			return userInfo
		},
		/**
     * @description: logout
     */
		async logout(goLogin = false) {
			if (this.getToken) {
				try {
					await doLogout()
				} catch {
					console.log('注销Token失败')
				}
			}
			this.setToken(undefined)
			this.setSessionTimeout(false)
			this.setUserInfo(null)
			goLogin && router.push(PageEnum.BASE_LOGIN)
		},

		/**
     * @description: Confirm before logging out
     */
		confirmLoginOut() {
			const { createConfirm } = useMessage()
			const { t } = useI18n()
			createConfirm({
				iconType: 'warning',
				title: () => h('span', t('sys.app.Tip')),
				content: () => h('span', t('sys.app.logoutMessage')),
				onOk: async () => {
					await this.logout(true)
				}
			})
		}
	}
})

// Need to be used outside the setup
export function useUserStoreWithOut() {
	return useUserStore(store)
}
