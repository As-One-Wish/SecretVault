using Info.Storage.Domain.Service.Shared;
using Info.Storage.Infra.Entity.Shared.Attributes;
using System.Reflection;

namespace Info.Storage.HttpApi.Host.Configurations
{
    /// <summary>
    /// 领域服务配置
    /// </summary>
    public static class AutoInjectConfig
    {
        /// <summary>
        /// 仓储层、领域层、服务层自动注入
        /// </summary>
        /// <param name="service"></param>
        public static void AddAutoInjectConfiguration(this IServiceCollection service)
        {
            // 注入的属性Key，区分服务
            /* 只有那些被分类为 "default" 或 "app" 的服务才会被注入,可以更加灵活地控制哪些服务需要被注入，从而实现更细粒度的依赖注入管理 */
            string[] injectKeys = ["default", "app"];
            // 反射获取程序所有加载类型
            /* 注：AppDomain.CurrentDomain.GetAssemblies()这个方法获取的是当前应用程序域已经加载的程序集 */
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Info.*.dll").Select(Assembly.LoadFrom).ToList();
            if (assemblies != null && assemblies.Count > 0)
            {
                // 安装 FreeSql.DbContext
                service.AddFreeRepository(null, assemblies.Where(d => d.FullName != null && d.FullName.Split(",")[0].EndsWith(".Repository")).ToArray());
                // 注入默认领域服务
                service.AddScoped(typeof(DefaultDomainService<,>), typeof(DefaultDomainService<,>));

                List<Type> types = assemblies.Where(d => d.FullName != null && (d.FullName.Split(",")[0].EndsWith(".Domain.Service") || d.FullName.Split(",")[0].EndsWith(".Application")))
                    .SelectMany(x => x.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttributes(typeof(AutoInjectAttribute), false).Length > 0)
                    .Where(t => injectKeys.Contains(t.GetCustomAttribute<AutoInjectAttribute>()?.Key))
                    .ToList();

                types.ForEach(impl =>
                {
                    // 获取该类所继承的所有接口
                    Type[] interfaces = impl.GetInterfaces();
                    // 获取该类注入的生命周期
                    ServiceLifetime? lifetime = impl.GetCustomAttribute<AutoInjectAttribute>()?.Lifetime;

                    interfaces.ToList().ForEach(i =>
                    {
                        switch (lifetime)
                        {
                            case ServiceLifetime.Singleton:
                                service.AddSingleton(i, impl);
                                break;

                            case ServiceLifetime.Transient:
                                service.AddTransient(i, impl);
                                break;

                            case ServiceLifetime.Scoped:
                                service.AddScoped(i, impl);
                                break;
                        }
                    });
                });
            }
        }
    }
}