namespace Hwj.SecretVault.Infra.Entity.Shared.Settings
{
    /// <summary>
    /// 数据库链接配置
    /// </summary>
    public record DbConnectionOptionConfig
    {
        /// <summary>
        /// 主库链接
        /// </summary>
        public string MasterConnection { get; set; } = string.Empty;
        /// <summary>
        /// 从库链接
        /// </summary>
        public List<string> SlaveConnections { get; set; } = new List<string>();
    }

    /// <summary>
    /// Consul配置类
    /// </summary>
    public record ConsulSettingConfig
    {
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// Consul服务地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Consul数据中心名称
        /// </summary>
        public string DataCenter { get; set; }
        /// <summary>
        /// 服务注册配置项
        /// </summary>
        public ServiceRegisterConfig ServiceRegisterConfig { get; set; }
    }
    /// <summary>
    /// Consul服务发现配置类
    /// </summary>
    public record ServiceRegisterConfig
    {
        /// <summary>
        /// 当前服务ID-唯一
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 服务组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 当前服务IP-本地控制台appsetting需要配置，webapi通过urls解析
        /// </summary>
        public string Ip { get; set; } = string.Empty;
        /// <summary>
        /// 当前服务端口-本地控制台appsetting需要配置，webapi通过urls
        /// </summary>
        public int Port { get; set; } = -1;
        /// <summary>
        /// 权重
        /// </summary>
        public short Weight { get; set; }
        /// <summary>
        /// 健康检查Url地址-get
        /// </summary>
        public string CheckHealthUrl { get; set; }
        /// <summary>
        /// 健康检查频率-单位秒
        /// </summary>
        public int CheckHealthInterval { get; set; }
        /// <summary>
        /// 健康检查超时-单位秒
        /// </summary>
        public int CheckHealthTimeout { get; set; }
        /// <summary>
        /// 检查失败后多久移除服务
        /// </summary>
        public int DeregisterAfter { get; set; }
    }
    /// <summary>
    /// Consul配置中心配置类
    /// </summary>
    public record ConsulConfigurationAppsetting
    {
        /// <summary>
        /// consul配置地址
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// 配置文件
        /// </summary>
        public Dictionary<string, string> DictionarySettingFiles { get; set; }
    }
}