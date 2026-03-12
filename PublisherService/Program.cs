using ArticleQueue.Extensions;
using Microsoft.AspNetCore.Builder;

namespace PublisherService;

public class Program
{
    public static async Task Main()
    {
        try
        {
            var builder = WebApplication.CreateBuilder();
            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();

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
        services.AddOpenApi();

        var options = services.MessageClientOptions(configuration);
        services.AddRabbitMqMessageClient(options);

        services.AddScoped<Services.PublisherService>();
    }
}