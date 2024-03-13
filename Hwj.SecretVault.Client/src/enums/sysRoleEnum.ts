/*
 * @Author: arcchen
 * @Description:
 * @Date: 2022-09-01 16:44:07
 * @LastEditTime: 2023-05-31 14:09:10
 * @FilePath: \CastcZhy.Tmp.WebClient.VbenAdmin\src\enums\sysRoleEnum.ts
 */
/**
 * @description: 角色标识枚举
 * @event:
 * @return {*}
 */
export enum SysRoleIdentEnum {
  Unknown = 0,
  System = 1,
  Custom = 2,
}
export const SysRoleIdentEnumMap = new Map([
  [SysRoleIdentEnum.Unknown, { label: '未知', name: 'Unknown', color: '#708090' }],
  [SysRoleIdentEnum.System, { label: '系统级', name: 'SuperAdmin', color: '#0926f2' }],
  [SysRoleIdentEnum.Custom, { label: '自定义', name: 'OrdinaryAdmin', color: '#008000' }],
]);
/**
 * @description: 用于系统权限认证的角色枚举
 * @event:
 * @param {*}
 * @return {*}
 */
export enum SysRoleEnum {
  Unknown = 'Unknown',
  SuperAdmin = 'SuperAdmin',
  OrdinaryAdmin = 'OrdinaryAdmin',
  OrdinaryUser = 'OrdinaryUser',
  OrdinaryTouristUser = 'OrdinaryTouristUser',
  InterfaceUser = 'InterfaceUser',
}

/**
 * @description:角色类型：0-未知 1-超级管理员 2-普通管理员 3-普通用户 4-普通只读用户 5-接口用户
    /// 超级管理员可以新增角色，配套修改前端和后端的对应枚举值
    /// 角色类型不和用户操作权限挂钩，至于WebAPI的Policy鉴权关联 
    ///  用于角色
 * @event: 
 * @return {*}
 */
export enum SysRoleTypeEnum {
  Unknown = 0,
  SuperAdmin = 1,
  OrdinaryAdmin = 2,
  OrdinaryUser = 3,
  OrdinaryTouristUser = 4,
  InterfaceUser = 5,
}

export const SysRoleTypeEnumMap = new Map([
  [
    SysRoleTypeEnum.Unknown,
    { label: '未知', name: 'Unknown', ident: SysRoleIdentEnum.System, color: '#708090' },
  ],
  [
    SysRoleTypeEnum.SuperAdmin,
    { label: '超级管理员', name: 'SuperAdmin', ident: SysRoleIdentEnum.System, color: '#006400' },
  ],
  [
    SysRoleTypeEnum.OrdinaryAdmin,
    { label: '普通管理员', name: 'OrdinaryAdmin', ident: SysRoleIdentEnum.System, color: '#008000' },
  ],
  [
    SysRoleTypeEnum.OrdinaryUser,
    { label: '普通用户', name: 'OrdinaryUser', ident: SysRoleIdentEnum.System, color: '#228B22' },
  ],
  [
    SysRoleTypeEnum.OrdinaryTouristUser,
    {
      label: '普通只读用户',
      name: 'OrdinaryTouristUser',
      ident: SysRoleIdentEnum.System,
      color: '#32CD32',
    },
  ],
  [
    SysRoleTypeEnum.InterfaceUser,
    { label: '接口用户', name: 'InterfaceUser', ident: SysRoleIdentEnum.System, color: '#8FBC8F' },
  ],
]);
