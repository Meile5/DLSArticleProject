using System.Diagnostics;
using ArticleQueue.Interfaces;
using MonitorService;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Shared;
using ArticlePublishedEvent = ArticleQueue.Models.Events.ArticlePublishedEvent;

namespace NewsletterService.Services;

public class NewsletterService(IMessageClient _client)
{
    public async Task NewsletterAsync(ArticlePublishedEvent request, PropagationContext parentContext, Baggage baggage)
    {
        
        using var activity = Monitoring.ActivitySource.StartActivity("Entered NewsletterAsync in NewsletterService", ActivityKind.Consumer, parentContext.ActivityContext);

        var finalEvent = new ArticlePublishedEvent()
        {
            ArticleId = request.ArticleId,
            Title = request.Title,
            Content = request.Content,
            AuthorName = request.AuthorName,
            PublishedAt = request.PublishedAt
        };
        
        //inject context before publishing finished event (for distributed tracing, so handlers that recieve this can get the context)
        var propagationContext = new PropagationContext(parentContext.ActivityContext, baggage);
        var propagator = new TraceContextPropagator();
        propagator.Inject(propagationContext, finalEvent,(bRequest, key, value) => bRequest.Header.Add(key, value) );

        await _client.Publish(finalEvent);
    }
}