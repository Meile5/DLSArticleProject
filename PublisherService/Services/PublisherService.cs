using System.Diagnostics;
using ArticleQueue.Interfaces;
using ArticleQueue.Models.Events;
using MonitorService;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using PublisherService.Entities;

namespace PublisherService.Services;

public class PublisherService(IMessageClient _client)
{
    public async Task PublishArticleAsync(PublishArticleRequest request)
    {
        using var activity = Monitoring.ActivitySource.StartActivity("PublishArticleAsync called in PublisherService");

        var articlePublished = new ArticlePublishedEvent
        {
            ArticleId = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            AuthorId = request.AuthorId,
            PublishedAt = DateTime.UtcNow
        };
        
        //inject context before publishing event (for tracing)
        var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;
        var propagationContext = new PropagationContext(activityContext, Baggage.Current);
        var propagator = new TraceContextPropagator();
        propagator.Inject(propagationContext, articlePublished,(bRequest, key, value) => bRequest.Header.Add(key, value) );

        await _client.Publish(articlePublished);
    }
}