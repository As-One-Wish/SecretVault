using CSRedis;

namespace Hwj.SecretVault.Utils.RedisHelper
{
    /// <summary>
    /// RedisServerHelper
    /// </summary>
    public class RedisServerHelper : RedisHelper<RedisServerHelper>
    {
        /// <summary>
        /// 初始化Redis
        /// </summary>
        /// <param name="redisConfig"></param>
        public static void Init(string redisConfig)
        {
            string[] cfg = redisConfig.Split('|');
            if (cfg[0] == "nosentinel")
            {
                if (cfg[1].Contains(";"))
                    Initialization(new CSRedisClient(null, cfg[1].Split(";")));
                else
                    Initialization(new CSRedisClient(cfg[1]));
            }
            else if (cfg[0] == "sentinel")
            {
                string[] sentinels = cfg[2].Split(";");
                Initialization(new CSRedisClient(cfg[1], sentinels));
            }
        }
    }
}