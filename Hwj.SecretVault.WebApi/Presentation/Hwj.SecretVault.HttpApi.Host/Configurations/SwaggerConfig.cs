using Info.Storage.HttpApi.Host.Filters;
using Microsoft.OpenApi.Models;

namespace Info.Storage.HttpApi.Host.Configurations
{
    /// <summary>
    /// Swagger配置
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Swagger注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="enableSwagger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddSwaggerConfiguration(this IServiceCollection services, bool enableSwagger = false)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (enableSwagger)
            {
                services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        // 描述信息
                        Description = "请输入带有Bearer的Token，形如\"Bearer {Token}\"",
                        // Header对应信息
                        Name = "Authorization",
                        // 验证信息
                        Type = SecuritySchemeType.ApiKey,
                        // 设置 API 密钥位置
                        In = ParameterLocation.Header,
                        // Bearer Token的格式
                        BearerFormat = "JWT",
                        // 身份验证方案的名称
                        Scheme = "Bearer"
                    });
                    // 指定方案应用范围
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new []
                            {
                                "readAccess",
                                "writeAccess"
                            }
                        }
                    });
                    // 允许为每个操作(即 API 方法)动态地添加或修改 Swagger 文档的内容
                    options.OperationFilter<SwaggerFileOperationFilter>();
                    // 添加并指定Swagger文档信息
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Hwj.SecretVault.HttpApi.Host", Version = "v1" });
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Hwj.SecretVault.HttpApi.Host.xml");

                    options.IncludeXmlComments(path, true); // 显示控制器层注释

                    options.OrderActionsBy(o => o.RelativePath); // 对action的名称进行排序
                });
            }
        }

        /// <summary>
        /// 安装Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="enableSwagger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UseSwaggerSetup(this IApplicationBuilder app, bool enableSwagger = false)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (enableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hwj.SecretVault.HttpApi.Host v1");
                });
            }
        }
    }
}