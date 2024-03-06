using Hwj.SecretVault.Utils.RedisHelper;
using Yitter.IdGenerator;

namespace Hwj.SecretVault.HttpApi.Host.Configurations
{
    /// <summary>
    /// 其他配置
    /// </summary>
    public static class OtherConfig
    {
        /// <summary>
        /// 其他配置注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddOtherConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            // 1.雪花算法 ID生成器
            var options = new IdGeneratorOptions(1); // 可将WorkId抽象为枚举，方便扩展
            YitIdHelper.SetIdGenerator(options);

            // 2.Redis初始化连接
            string? strRedisConnection = configuration.GetValue<string>("RedisConnectionSettings:redis-server");
            RedisServerHelper.Init(strRedisConnection);
        }
    }
}