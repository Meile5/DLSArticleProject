using System.Diagnostics;
using ArticleQueue.Interfaces;
using ArticleQueue.Models.Events;
using MonitorService;
using NewsletterService.Entities;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace NewsletterService.Services;

public class NewsletterService(IMessageClient _client)
{
    public async Task NewsletterAsync(ArticlePublishedEvent request, PropagationContext parentContext, Baggage baggage)
    {
        
        using var activity = Monitoring.ActivitySource.StartActivity("Entered HandleAsync in NewsletterHandler");

        var finalEvent = new NewsletterEvent
        {
            ArticleId = request.ArticleId,
            Title = request.Title,
            Content = request.Content,
            AuthorId = request.AuthorId,
            PublishedAt = request.PublishedAt
        };
        
        //inject context before publishing finished event (for distributed tracing, so handlers that recieve this can get the context)
        var propagationContext = new PropagationContext(parentContext.ActivityContext, baggage);
        var propagator = new TraceContextPropagator();
        propagator.Inject(propagationContext, finalEvent,(bRequest, key, value) => bRequest.Header.Add(key, value) );

        await _client.Publish(finalEvent);
    }
}