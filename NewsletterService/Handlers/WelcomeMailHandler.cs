using System.Diagnostics;
using ArticleQueue.Interfaces;
using MonitorService;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Serilog;
using Shared;
using SubscriberQueue.Events;

namespace NewsletterService.Handlers;

public class WelcomeMailHandler(Services.NewsletterService service) : IMessageHandler<NewSubscriberSuccessEvent>
{
    public async Task HandleAsync(NewSubscriberSuccessEvent message, CancellationToken ct)
    {
        //getting the context from event that was published (for distributed tracing)
        var propagator = new TraceContextPropagator();
        var parentContext = propagator.Extract(default, message, (request, s) =>
        {
            return new List<string?>(new[] { request.Header.ContainsKey(s) ? request.Header[s].ToString() : string.Empty});
        });
        Baggage.Current = parentContext.Baggage;
        using var activity = Monitoring.ActivitySource.StartActivity("Entered HandleAsync in WelcomeMailHandler", ActivityKind.Consumer, parentContext.ActivityContext);
        
        //serilog logging
        Log.Logger.Debug("Entered HandleAsync in WelcomeMailHandler");
        
        
        await service.SendWelcomeMail(message, parentContext, Baggage.Current);
    }
}