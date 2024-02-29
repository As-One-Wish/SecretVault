using Info.Storage.HttpApi.Host.Configurations;

namespace Info.Storage.HttpApi.Host
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
            builder.Configuration.AddJsonFile("localAppsettings.json");

            //DbConnectionOptionConfig? oDbConnectionOptionConfig = builder.Configuration.GetSection("DbConnectionStrings:DbOtherPostgresqlConnectionString").Get<DbConnectionOptionConfig>();

            // Add services to the container.

            bool enableSwagger = bool.Parse(builder.Configuration["EnableSwagger"] ?? "false");

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

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