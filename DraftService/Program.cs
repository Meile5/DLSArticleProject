using DraftService.AppOptionsPattern;
using DraftService.Database;
using DraftService.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static async Task Main()
    {
        try
        {
            var builder = WebApplication.CreateBuilder();
            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            // Migrate/create database
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DraftContext>();
                db.Database.Migrate();
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUi(options =>
                {
                    options.DocumentPath = "/openapi/v1.json";
                });
            }

            app.MapControllers();

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddAppOptions(configuration);
        services.AddOpenApi();

        services.AddDbContext<DraftContext>(options =>
            options.UseSqlServer(configuration["AppOptions:DbConnectionString"]));

        services.AddScoped<DraftRepository>();
        services.AddScoped<DraftService.Service.DraftService>();
    }
}