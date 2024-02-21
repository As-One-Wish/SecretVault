using Info.Storage.Infa.Entity.Shared.Settings;
using Info.Storage.Infa.Repository.Extension;
using Info.Storage.Utils.CommonHelper.Helpers;
using Microsoft.Extensions.Configuration;

namespace Info.Storage.Infa.Repository.Shared
{
    public class SingleFreeSql : BaseSingleFreeSql<string>
    {
        /// <summary>
        /// 注册FreeSql
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public void RegisterFreeSql(IConfiguration configuration)
        {
            Register("app", () => new Lazy<IFreeSql>(() =>
            {
                DbConnectionOptionConfig oFreeSqlDbConnectionItemConfig = configuration.GetSection("DbConnectionStrings:DbAppPostgresqlConnectionString").Get<DbConnectionOptionConfig>();
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

                    IFreeSql _freeSql = freeBuilder.Build();
                    // 语句执行前对所执行的Sql语句进行操作
                    _freeSql.Aop.CurdBefore += (s, e) =>
                    {
                        string strSql = e.Sql;
                    };

                    return _freeSql;
                }
                else
                    return null;
            }, true)?.Value);
        }
    }
}