using System.Diagnostics;
using MonitorService;
using NewsletterService.Clients;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Serilog;
using Shared.Events;

namespace NewsletterService.Services;

public class NewsletterService(ISubscriberClient _subscriberClient)
{
    public async Task NewsletterAsync(ArticlePublishedEvent request, PropagationContext parentContext, Baggage baggage)
    {
        //opentelemetry activity start
        using var activity = Monitoring.ActivitySource.StartActivity("Entered NewsletterAsync in NewsletterService", ActivityKind.Consumer, parentContext.ActivityContext);
        
        //serilog logging
        Log.Logger.Debug("Entered HandleAsync in NewsletterHandler");

        var subscribers = await _subscriberClient.GetSubscribersAsync();
        foreach (var sub in subscribers)
        {
            Log.Logger.Information("Sending newsletter '{Title}' to {Email}", request.Title, sub.Email);
        }
    }

    public async Task SendWelcomeMail(NewSubscriberSuccessEvent subEvent, PropagationContext parentContext, Baggage baggage)
    {
        //opentelemetry activity start
        using var activity = Monitoring.ActivitySource.StartActivity("Entered SendWelcomeMail in NewsletterService", ActivityKind.Consumer, parentContext.ActivityContext);
        
        //serilog logging
        Log.Logger.Debug("Entered SendWelcomeMail in NewsletterService");

        Log.Logger.Information("Sending welcome mail to {Email}", subEvent.Email);
    }
}