using Hwj.Aow.Infra.Repository.Extension;
using Hwj.SecretVault.Infra.Repository.Shared;

namespace Hwj.SecretVault.HttpApi.Host.Configurations
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