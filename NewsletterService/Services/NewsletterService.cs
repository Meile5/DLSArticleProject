using ArticleQueue.Interfaces;
using ArticleQueue.Models.Events;
using MonitorService;
using NewsletterService.Entities;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace NewsletterService.Services;

public class NewsletterService(IMessageClient _client)
{
    public async Task NewsletterAsync(ArticlePublishedEvent request)
    {
        var propagator = new TraceContextPropagator();
        var parentContext = propagator.Extract(default, request, (request, s) =>
        {
            return new List<string?>(new[] { request.Header.ContainsKey(s) ? request.Header[s].ToString() : string.Empty});
        });
        Baggage.Current = parentContext.Baggage;
        using var activity = Monitoring.ActivitySource.StartActivity("Entered HandleAsync in NewsletterHandler");

        var finalEvent = new NewsletterEvent
        {
            ArticleId = request.ArticleId,
            Title = request.Title,
            Content = request.Content,
            AuthorId = request.AuthorId,
            PublishedAt = request.PublishedAt
        };
        
        //inject context before publishing finished event (for tracing)
        var propagationContext = new PropagationContext(parentContext.ActivityContext, Baggage.Current);
        propagator.Inject(propagationContext, finalEvent,(bRequest, key, value) => bRequest.Header.Add(key, value) );

        await _client.Publish(finalEvent);
    }
}