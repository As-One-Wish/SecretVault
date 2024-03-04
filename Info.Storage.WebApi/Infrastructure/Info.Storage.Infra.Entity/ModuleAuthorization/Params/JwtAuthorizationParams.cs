namespace Info.Storage.Infra.Entity.ModuleAuthorization.Params
{
    public record JwtLoginParam
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
    }
}