using Hwj.SecretVault.HttpApi.Host.Configurations;
using Hwj.SecretVault.Infra.Entity.Shared.Settings;
using Winton.Extensions.Configuration.Consul;

namespace Hwj.SecretVault.HttpApi.Host
{
    /// <summary>
    /// 主程序
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 程序入口
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region 方式1：Consul配置

            builder.Configuration.AddJsonFile("consulAppsettings.json");

            ConsulConfigurationAppsetting? consulConfigurationAppsetting = builder.Configuration.GetSection("ConsulConfigurationAppsetting").Get<ConsulConfigurationAppsetting>();
            if (consulConfigurationAppsetting != null)
            {
                foreach (var itemSetting in consulConfigurationAppsetting.DictionarySettingFiles)
                {
                    builder.Configuration.AddConsul(itemSetting.Value, op =>
                    {
                        op.ConsulConfigurationOptions = cc => cc.Address = new Uri(consulConfigurationAppsetting.Server);
                        op.ReloadOnChange = true;
                    });
                }
            }

            #endregion 方式1：Consul配置

            #region 方式2：本地json文件配置

            //builder.Configuration.AddJsonFile("localAppsettings.json");

            #endregion 方式2：本地json文件配置

            bool enableSwagger = bool.Parse(builder.Configuration["EnableSwagger"] ?? "false");

            #region Add services to the container.

            // 配置 Consul-服务发现
            builder.Services.AddConsulConfiguration(builder.Configuration);
            // 添加控制器服务
            builder.Services.AddControllers();
            // 配置自动注入
            builder.Services.AddAutoInjectConfiguration();
            // 配置 FreeSql
            builder.Services.AddFreeSqlConfiguration(builder.Configuration);
            // 配置 AutoMapper
            builder.Services.AddAutoMapperConfiguration();
            // 配置Jwt身份验证
            builder.Services.AddJwtConfiguration(builder.Configuration);
            // 配置其他依赖
            builder.Services.AddOtherConfiguration(builder.Configuration);
            // 配置Swagger
            builder.Services.AddSwaggerConfiguration(enableSwagger);
            // 配置验证器
            builder.Services.AddValidatorConfiguration();

            #endregion Add services to the container.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
                app.UseSwaggerSetup(enableSwagger);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication(); // 鉴权-读取请求方所带用户信息
            app.UseAuthorization(); // 授权-根据用户信息检测用户权限

            app.MapControllers();

            app.Run();
        }
    }
}