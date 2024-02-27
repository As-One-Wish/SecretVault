namespace Info.Storage.Infa.Entity.ModuleAuthorization.Params
{
    public record JwtLoginParam
    {
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
    }
}