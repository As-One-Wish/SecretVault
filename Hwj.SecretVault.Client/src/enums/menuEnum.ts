/*
 * @Author: arcchen
 * @Description:
 * @Date: 2022-09-01 16:44:07
 * @LastEditTime: 2022-12-29 17:35:15
 * @FilePath: \CastcZhy.Tmp.WebClient.VbenAdmin\src\enums\menuEnum.ts
 */
/**
 * @description: menu type
 */
export enum MenuTypeEnum {
  // left menu
  SIDEBAR = 'sidebar',

  MIX_SIDEBAR = 'mix-sidebar',
  // mixin menu
  MIX = 'mix',
  // top menu
  TOP_MENU = 'top-menu',
}

// 折叠触发器位置
export enum TriggerEnum {
  // 不显示
  NONE = 'NONE',
  // 菜单底部
  FOOTER = 'FOOTER',
  // 头部
  HEADER = 'HEADER',
}

export type Mode = 'vertical' | 'horizontal';

// menu mode
export enum MenuModeEnum {
  VERTICAL = 'vertical', //垂直菜单menu sider,显示
  HORIZONTAL = 'horizontal', //水平菜单
  // VERTICAL_RIGHT = 'vertical-right',
  INLINE = 'inline',
}

export enum MenuSplitTyeEnum {
  NONE,
  TOP,
  LEFT,
}

export enum TopMenuAlignEnum {
  CENTER = 'center',
  START = 'start',
  END = 'end',
}

export enum MixSidebarTriggerEnum {
  HOVER = 'hover',
  CLICK = 'click',
}
