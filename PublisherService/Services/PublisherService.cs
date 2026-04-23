using System.Diagnostics;
using ArticleQueue.Interfaces;
using Shared.Events;
using MonitorService;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using PublisherService.Entities;
using Serilog;

namespace PublisherService.Services;

public class PublisherService(IMessageClient _client)
{
    public async Task PublishArticleAsync(PublishArticleRequest request)
    {
        using var activity = Monitoring.ActivitySource.StartActivity("PublishArticleAsync called in PublisherService");

        Log.Logger.Debug("PublishArticleAsync called in PublisherService");

        
        ArticlePublishedEvent articlePublished = new ArticlePublishedEvent
        {
            ArticleId = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            AuthorName = request.AuthorName,
            PublishedAt = DateTime.UtcNow
        };
        
        //inject context before publishing event (for distributed tracing, so handlers that recieve this can get the context)
        var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;
        var propagationContext = new PropagationContext(activityContext, Baggage.Current);
        var propagator = new TraceContextPropagator();
        propagator.Inject(propagationContext, articlePublished,(bRequest, key, value) => bRequest.Header.Add(key, value) );

        await _client.Publish(articlePublished);
    }
}