using Info.Storage.HttpApi.Host.Configurations;

namespace Info.Storage.HttpApi.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("localAppsettings.json");

            //DbConnectionOptionConfig? oDbConnectionOptionConfig = builder.Configuration.GetSection("DbConnectionStrings:DbOtherPostgresqlConnectionString").Get<DbConnectionOptionConfig>();

            // Add services to the container.

            bool enableSwagger = bool.Parse(builder.Configuration["EnableSwagger"] ?? "false");

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            // ≈‰÷√◊‘∂Ø◊¢»Î
            builder.Services.AddAutoInjectConfiguration();
            // ≈‰÷√ FreeSql
            builder.Services.AddFreeSqlConfiguration(builder.Configuration);
            // ≈‰÷√ AutoMapper
            builder.Services.AddAutoMapperConfiguration();
            // ≈‰÷√∆‰À˚“¿¿µ
            builder.Services.AddOtherConfiguration(builder.Configuration);
            // ≈‰÷√Swagger
            builder.Services.AddSwaggerConfiguration(enableSwagger);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
                app.UseSwaggerSetup(enableSwagger);

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}