using Microsoft.AspNetCore.Builder;
using ProfanityService.AppOptionsPattern;
using ProfanityService.Database;

namespace ProfanityService;

public class Program
{
    public static async Task Main()
    {
        try
        {
            var builder = WebApplication.CreateBuilder();
            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            
            // <snippet_UseSwagger>
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUi(options =>
                {
                    options.DocumentPath = "/openapi/v1.json";
                });
            }
            // </snippet_UseSwagger>
            
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
        services.AddDataSourceAndRepositories();
        services.AddOpenApi();
        
        services.AddScoped<Service.ProfanityService>();

    }
}