using System.Diagnostics;
using ArticleQueue.Interfaces;
using ArticleQueue.Models.Events;
using MonitorService;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace NewsletterService.Handlers;

public class NewsletterHandler(Services.NewsletterService service): IMessageHandler<ArticlePublishedEvent>{

    public async Task HandleAsync(ArticlePublishedEvent message, CancellationToken ct)
    {
        //getting the context from event that was published (for distributed tracing)
        var propagator = new TraceContextPropagator();
        var parentContext = propagator.Extract(default, message, (request, s) =>
        {
            return new List<string?>(new[] { request.Header.ContainsKey(s) ? request.Header[s].ToString() : string.Empty});
        });
        Baggage.Current = parentContext.Baggage;
        using var activity = Monitoring.ActivitySource.StartActivity("Entered HandleAsync in NewsletterHandler", ActivityKind.Consumer, parentContext.ActivityContext);
        
        //last two things here are for tracing
        await service.NewsletterAsync(message, parentContext, Baggage.Current);
    }
    
}