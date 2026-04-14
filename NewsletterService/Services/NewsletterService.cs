using System.Diagnostics;
using ArticleQueue.Interfaces;
using MonitorService;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Serilog;
using Shared;
using SubscriberQueue.Events;
using ArticlePublishedEvent = ArticleQueue.Models.Events.ArticlePublishedEvent;

namespace NewsletterService.Services;

public class NewsletterService(IMessageClient _client)
{
    public async Task NewsletterAsync(ArticlePublishedEvent request, PropagationContext parentContext, Baggage baggage)
    {
        //opentelemetry activity start
        using var activity = Monitoring.ActivitySource.StartActivity("Entered NewsletterAsync in NewsletterService", ActivityKind.Consumer, parentContext.ActivityContext);
        
        //serilog logging
        Log.Logger.Debug("Entered HandleAsync in NewsletterHandler");

        var finalEvent = new NewsletterEvent()
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

    public async Task SendWelcomeMail(NewSubscriberSuccessEvent subEvent, PropagationContext parentContext, Baggage baggage)
    {
        //opentelemetry activity start
        using var activity = Monitoring.ActivitySource.StartActivity("Entered SendWelcomeMail in NewsletterService", ActivityKind.Consumer, parentContext.ActivityContext);
        
        //serilog logging
        Log.Logger.Debug("Entered HandleAsync in NewsletterHandler");
        
        WelcomeMailEvent welcomeMailEvent = new WelcomeMailEvent
        {
            SubscriberId = subEvent.SubscriberId,
            Email = subEvent.Email,
            Title = "Welcome to Happy Headlines! We hope you enjoy your stay~",
            Content = "Here at Happy Headlines, we blah blah blah blah blah blah blah blah blah..."
        };

        //inject context before publishing finished event (for distributed tracing, so handlers that recieve this can get the context)
        var propagationContext = new PropagationContext(parentContext.ActivityContext, baggage);
        var propagator = new TraceContextPropagator();
        propagator.Inject(propagationContext, welcomeMailEvent,(bRequest, key, value) => bRequest.Header.Add(key, value) );

        
        await _client.Publish(welcomeMailEvent);
    }
}