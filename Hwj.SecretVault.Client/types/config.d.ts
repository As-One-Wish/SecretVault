import { PermissionModeEnum, SetttingPositionEnum } from '@/enums/appEnum'
import { CacheTypeEnum } from '@/enums/cacheEnum'

export interface ProjectConfig {
  // 权限相关信息的存储位置
  permissionCacheType: CacheTypeEnum
  // 是否显示设置按钮
  showSettingButton: boolean
  // 设置按钮显示位置
  settingButtonPosition: SetttingPositionEnum
  // 是否显示主题切换按钮
  showDarkModeToggle: boolean
  // 权限模式
  permissionMode: PermissionModeEnum
  
}