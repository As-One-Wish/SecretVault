using Info.Storage.Infa.Repository.Extension;
using Info.Storage.Infa.Repository.Shared;

namespace Info.Storage.HttpApi.Host.Configurations
{
    /// <summary>
    /// FreeSql配置
    /// </summary>
    public static class FreeSqlConfig
    {
        /// <summary>
        /// FreeSql 注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddFreeSqlConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var singleFreeSql = new SingleFreeSql();
            singleFreeSql.RegisterFreeSql(configuration);
            services.AddSingleton<IBaseSingleFreeSql<DbEnum>>(singleFreeSql);
        }
    }
}