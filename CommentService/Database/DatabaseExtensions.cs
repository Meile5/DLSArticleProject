using CommentService.AppOptionsPattern;
using CommentService.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CommentService.Database;

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
        
        services.AddScoped<CommentsRepo>();

        return services;
    }
}