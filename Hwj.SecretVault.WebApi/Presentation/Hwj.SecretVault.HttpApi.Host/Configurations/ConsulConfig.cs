using Consul;
using Hwj.SecretVault.Infra.Entity.Shared.Settings;
using System.Diagnostics;

namespace Hwj.SecretVault.HttpApi.Host.Configurations
{
    /// <summary>
    /// Consul服务发现配置
    /// </summary>
    public static class ConsulConfig
    {
        /// <summary>
        /// 添加Consul配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddConsulConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            ConsulSettingConfig? consulSettingConfig = configuration.GetSection("ConsulSettings").Get<ConsulSettingConfig>();
            if (consulSettingConfig != null && consulSettingConfig.Enable)
            {
                string? urls = configuration.GetValue<string>("urls");
                string? tag = configuration.GetValue<string>("tag");

                // 解析实际Ip和Port
                string? realIp = string.Empty;
                int realPort = -1;
                // 如果没有配置，则解析Url
                if (string.IsNullOrWhiteSpace(consulSettingConfig.ServiceRegisterConfig.Ip))
                    realIp = urls?.Replace("//", "").Split(":")[1];
                else
                    realIp = consulSettingConfig.ServiceRegisterConfig.Ip;

                if (consulSettingConfig.ServiceRegisterConfig.Port == -1)
                    realPort = Convert.ToInt32(urls?.Replace("//", "").Split(":")[2]);
                else
                    realPort = consulSettingConfig.ServiceRegisterConfig.Port;

                ConsulClient client = new ConsulClient(c =>
                {
                    c.Address = new Uri(consulSettingConfig.Address);
                    c.Datacenter = consulSettingConfig.DataCenter;
                });

                client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = $"{urls}({tag})", // 唯一的服务ID
                    Name = consulSettingConfig.ServiceRegisterConfig.GroupName, // 组名称
                    Address = realIp ?? "127.0.0.1", // 服务的实际IP地址
                    Port = realPort,
                    Tags = new string[] { urls ?? string.Empty, tag ?? "server1" },
                    Check = new AgentServiceCheck()
                    {
                        Interval = TimeSpan.FromSeconds(consulSettingConfig.ServiceRegisterConfig.CheckHealthInterval),
                        HTTP = $"{urls}{consulSettingConfig.ServiceRegisterConfig.CheckHealthUrl}",
                        Notes = $"Checks Health Status On {urls}",
                        Timeout = TimeSpan.FromSeconds(consulSettingConfig.ServiceRegisterConfig.CheckHealthTimeout),
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(consulSettingConfig.ServiceRegisterConfig.DeregisterAfter)
                    }
                });

                services.AddSingleton<IConsulClient>(client);
            }
        }
    }
}