using ArticleService.Dtos;
using ArticleService.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared;

namespace ArticleService.BackgroundServices;

public class EasyNetQSubscriberService : BackgroundService
{
    private readonly IMessageClient _messageClient;
    private readonly IServiceProvider _serviceProvider; 
    private readonly ILogger<EasyNetQSubscriberService> _logger;
    private const string SubscriptionId = "ArticleServiceSubscriber";

    public EasyNetQSubscriberService(
        IMessageClient messageClient,
        IServiceProvider serviceProvider, 
        ILogger<EasyNetQSubscriberService> logger)
    {
        _messageClient = messageClient;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Subscribe to the queue
        await _messageClient.SubscribeAsync<ArticlePublishedEvent>(SubscriptionId, async evt =>
        {
            if (stoppingToken.IsCancellationRequested)
                return;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var articleService = scope.ServiceProvider.GetRequiredService<IArticleService>();

                var dto = new ArticleCreateDto
                {
                    Title = evt.Title,
                    Contents = evt.Contents,
                    PublishingDate = evt.PublishingDate,
                    AuthorName = evt.AuthorName
                };

                await articleService.CreateArticleAsync(dto);
                _logger.LogInformation("Article saved from event: {Title}", evt.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving article from event: {Title}", evt.Title);
            }
        });

        // Keep the background service running until cancellation
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}