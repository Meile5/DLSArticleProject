using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProfanityService.AppOptionsPattern;
using ProfanityService.Repositories;


namespace ProfanityService.Database;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDataSourceAndRepositories(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>((service, options) =>
        {
            var provider = services.BuildServiceProvider();
            options.UseSqlServer(
                provider.GetRequiredService<IOptionsMonitor<AppOptions>>().CurrentValue.DbConnectionString);
            options.EnableSensitiveDataLogging();
        });
        
        services.AddScoped<ProfanityRepo>();

        return services;
    }
}