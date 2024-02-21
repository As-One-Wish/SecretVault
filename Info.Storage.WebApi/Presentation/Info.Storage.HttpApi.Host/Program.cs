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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // ���� FreeSql
            builder.Services.AddFreeSqlConfiguration(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}