﻿namespace Hwj.SecretVault.Infra.Entity.ModuleAuthorization.Dtos
{
    public record struct JwtUserDto
    {
        /// <summary>
        /// 当使用FreeSQL返回Dto时必须构造函数
        /// </summary>
        public JwtUserDto()
        {
            UserId = -1;
            UserName = string.Empty;
            Account = string.Empty;
            RoleId = -1;
            RoleName = string.Empty;
        }
        /// <summary>
        /// 主键
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }

    public record struct JwtAuthorizationDto
    {
        /// <summary>
        /// 授权时间
        /// </summary>
        public long AuthTime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public long ExpireTime { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }
}