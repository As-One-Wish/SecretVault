namespace Info.Storage.Infra.Entity.Shared.Settings
{
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
}