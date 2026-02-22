
using CommentService.AppOptionsPattern;
using CommentService.Clients;
using CommentService.Database;
using CommentService.Service;
using Microsoft.EntityFrameworkCore;

namespace CommentService;

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
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }
            
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
        
        // Configure AppOptions 
        services.AddAppOptions(configuration);
        
        // Add database
        services.AddDataSourceAndRepositories();
        
        services.AddOpenApi();
        
        // Add Services
        services.AddScoped<CommentsService>();
        
        // Add Profanity HTTP Client with circuit breaker
        services.AddHttpClient<IProfanityClient, ProfanityClient>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.Timeout = TimeSpan.FromSeconds(3); // fail faster
        });

    }
}