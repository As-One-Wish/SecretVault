namespace Hwj.SecretVault.Infra.Entity.Shared.Params
{
    public record BaseParam
    {
        /// <summary>
        /// 前多少条
        /// </summary>
        public int? Top { get; set; }
        /// <summary>
        /// 每页多少条
        /// </summary>
        public int? PageSize { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int? PageIndex { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderBy { get; set; }
        /// <summary>
        /// 排序方式 ascend(升序)|descend(降序)
        /// </summary>
        public string OrderByType { get; set; }
    }
}