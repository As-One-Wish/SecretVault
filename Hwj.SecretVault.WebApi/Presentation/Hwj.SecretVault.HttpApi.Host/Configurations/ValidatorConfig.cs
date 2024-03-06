using System.Reflection;
using FluentValidation;

namespace Hwj.SecretVault.HttpApi.Host.Configurations
{
    /// <summary>
    /// 验证器注入
    /// </summary>
    public static class ValidatorConfig
    {
        /// <summary>
        /// 添加验证器的批量注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddValidatorConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var validatorList = GetFluentValidationValidator();
            foreach (var validator in validatorList)
                services.AddValidatorsFromAssemblyContaining(validator, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// 获取所有 FluentValidation 类
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetFluentValidationValidator()
        {
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Hwj.*.Validator.dll").Select(Assembly.LoadFrom).ToList();
            if (assemblies != null && assemblies.Count > 0)
            {
                List<Type> types = assemblies.Where(t => t.FullName != null && t.FullName.Split(",")[0].EndsWith("Validator"))
                    .SelectMany(t => t.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.Name == "AbstractValidator`1")
                    .ToList();
                return types;
            }
            else
                return Enumerable.Empty<Type>();
        }
    }
}