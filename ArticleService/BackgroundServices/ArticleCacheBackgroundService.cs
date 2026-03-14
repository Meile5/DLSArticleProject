namespace ArticleService.BackgroundServices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Database;

public class ArticleCacheBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ArticleCacheService _cacheService;

    public ArticleCacheBackgroundService(IServiceScopeFactory scopeFactory, ArticleCacheService cacheService)
    {
        _scopeFactory = scopeFactory;
        _cacheService = cacheService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IArticleRepository>();

            var fromDate = DateTime.UtcNow.AddDays(-14);
            var articles = (await repository.GetAllAsync())
                .Where(a => a.PublishingDate >= fromDate);

            _cacheService.SetArticles(articles);

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}