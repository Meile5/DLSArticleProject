using System.Text.Json;
using ArticleService.Dtos;
using ArticleService.Services;
using EasyNetQ;
using Microsoft.Extensions.Hosting;

namespace ArticleService.BackgroundServices;

public class EasyNetQSubscriberService : BackgroundService
{
    private readonly IMessageClient _messageClient;
    private readonly IArticleService _articleService;
    private const string SubscriptionId = "ArticleServiceSubscriber"; // Unique ID for this subscriber

    public EasyNetQSubscriberService(IMessageClient messageClient, IArticleService articleService)
    {
        _messageClient = messageClient;
        _articleService = articleService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Subscribe to the queue
        _messageClient.SubscribeAsync<ArticleCreateDto>(SubscriptionId, async articleDto =>
        {
            try
            {
                await _articleService.CreateArticleAsync(articleDto);
                Console.WriteLine($"Article saved: {articleDto.Title}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving article: {ex.Message}");
            }
        });

        return Task.CompletedTask;
    }
}