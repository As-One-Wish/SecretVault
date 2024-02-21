using Info.Storage.Infa.Entity.Shared.Settings;
using Info.Storage.Infa.Repository.Extension;
using Info.Storage.Infa.Repository.Shared;
using Info.Storage.Utils.CommonHelper.Helpers;

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
            services.AddSingleton<IBaseSingleFreeSql<string>>(singleFreeSql);
        }
    }
}