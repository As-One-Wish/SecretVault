using Info.Storage.Infa.Entity.Shared.Settings;
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

            DbConnectionOptionConfig? oFreeSqlDbConnectionItemConfig = configuration.GetSection("DbConnectionStrings:DbAppPostgresqlConnectionString").Get<DbConnectionOptionConfig>();
            IFreeSql? freeSql = null;
            if (oFreeSqlDbConnectionItemConfig?.MasterConnection != null)
            {
                var freeBuilder = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.PostgreSQL, oFreeSqlDbConnectionItemConfig.MasterConnection)
                .UseAutoSyncStructure(false) // 关闭自动同步结构
                .UseAdoConnectionPool(true) // 启用连接池
                .UseGenerateCommandParameterWithLambda(true) // 启用基于Lambda表达式的命令参数生成
                .UseLazyLoading(true) // 启用延迟加载
                .UseMonitorCommand(cmd => { LogHelper.Info(cmd.CommandText); }); // 监控SQL命令执行，并记录信息

                // 是否开启读写分离配置
                if (oFreeSqlDbConnectionItemConfig.SlaveConnections.Count > 0)
                    freeBuilder.UseSlave(oFreeSqlDbConnectionItemConfig.SlaveConnections.ToArray());

                freeSql = freeBuilder.Build();
                // 语句执行前对所执行的Sql语句进行操作
                freeSql.Aop.CurdBefore += (s, e) =>
                {
                    string strSql = e.Sql;
                };
            }

            if (freeSql != null)
                services.AddSingleton(freeSql);
        }
    }
}