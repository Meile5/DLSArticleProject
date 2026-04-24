using System.Diagnostics;
using ArticleQueue.Interfaces;
using ArticleQueue.Models;
using Shared.Events;
using ArticleService.Dtos;
using ArticleService.Services;
using MonitorService;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Serilog;

namespace ArticleService.BackgroundServices;

public class ArticleSubscriber : BackgroundService
{
    private readonly IMessageClient _messageClient;
    private readonly IServiceProvider _serviceProvider; 
    private readonly ILogger<ArticleSubscriber> _logger;
    private const string SubscriptionId = "ArticleServiceSubscriber";

    public ArticleSubscriber(
        IMessageClient messageClient,
        IServiceProvider serviceProvider, 
        ILogger<ArticleSubscriber> logger)
    {
        _messageClient = messageClient;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageClient.Subscribe(
            SubscriptionId,
            new MessageHandler<ArticlePublishedEvent>(evt =>
            {
                _ = HandleEventAsync(evt, stoppingToken);
            }),
            stoppingToken
        );

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task HandleEventAsync(ArticlePublishedEvent evt, CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested)
            return;

        try
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "development")
            {
                //logging & tracing
                var propagator = new TraceContextPropagator();
                var parentContext = propagator.Extract(default, evt, (request, s) =>
                {
                    return new List<string?>(new[] { request.Header.ContainsKey(s) ? request.Header[s].ToString() : string.Empty});
                });
                Baggage.Current = parentContext.Baggage;
                using var activity = Monitoring.ActivitySource
                    .StartActivity("Entered HandleEventAsync in ArticleSubscriber", ActivityKind.Consumer, parentContext.ActivityContext);

                Log.Logger.Debug("Entered HandleEventAsync in ArticleSubscriber");
            }
            
            using var scope = _serviceProvider.CreateScope();
            var articleService = scope.ServiceProvider.GetRequiredService<IArticleService>();

            var dto = new ArticleCreateDto
            {
                Title = evt.Title,
                Contents = evt.Content,
                PublishingDate = evt.PublishedAt,
                AuthorName = evt.AuthorName
            };

            await articleService.CreateArticleAsync(dto);
            _logger.LogInformation("Article saved from event: {Title}", evt.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving article from event: {Title}", evt.Title);
        }
    }

}