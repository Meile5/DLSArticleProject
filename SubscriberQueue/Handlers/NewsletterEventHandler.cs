using System.Diagnostics;
using ArticleQueue.Interfaces;
using Serilog;
using Shared;
using SubscriberQueue.Models;

namespace SubscriberQueue.Handlers;

public class NewsletterEventHandler(SubscriberList subscribeList) : IMessageHandler<NewsletterEvent>
{
    public async Task HandleAsync(NewsletterEvent message, CancellationToken ct)
    {
        //Log.Logger.Debug("Entered HandleAsync in NewsletterEVENTHandler");
        var subs = subscribeList.GetList();
        List<Subscriber> activeSubs = subs.Where(s => s.isActive).ToList();

        
        foreach (var sub in activeSubs)
        {
            sub.RecieveMail(message.Content);
        }
        
    }
}