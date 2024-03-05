using AutoMapper;
using System.Reflection;

namespace Info.Storage.HttpApi.Host.Configurations
{
    /// <summary>
    /// AutoMapper 配置
    /// </summary>
    public static class AutoMapperConfig
    {
        /// <summary>
        /// AutoMapper注入
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var autoMapperProfileList = GetAutoMapperProfiles();
            // 注入AutoMapper策略
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                foreach (var profile in autoMapperProfileList)
                    cfg.AddProfile(profile);
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }

        /// <summary>
        /// 利用反射获取AutoMapper类型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetAutoMapperProfiles()
        {
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Info.*.AutoMapper.dll").Select(Assembly.LoadFrom).ToList();
            if (assemblies != null && assemblies.Count > 0)
            {
                List<Type> types = assemblies.Where(d => d.FullName != null && d.FullName.Split(',')[0].EndsWith("AutoMapper"))
                    .SelectMany(x => x.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract && (t.BaseType?.FullName == "AutoMapper.Profile"))
                    .ToList();
                return types;
            }
            else
                return Enumerable.Empty<Type>();
        }
    }
}