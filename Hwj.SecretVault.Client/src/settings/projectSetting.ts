/** 
 * @description 项目配置文件
 * @fileName projectSetting.ts 
 * @author HWJ 
 * @date 2024-03-11 16:37:41
 */
import type { ProjectConfig } from '#/config'
import { PermissionModeEnum, SetttingPositionEnum } from '@/enums/appEnum'
import { CacheTypeEnum } from '@/enums/cacheEnum'

// 改动后需要清空浏览器缓存
const setting: ProjectConfig = {
	permissionCacheType: CacheTypeEnum.LOCAL,
	showSettingButton: false,
	settingButtonPosition: SetttingPositionEnum.AUTO,
	showDarkModeToggle: false,
	permissionMode: PermissionModeEnum.ROLE
}

export default setting