using ArticleQueue.Interfaces;
using Shared;
using SubscriberQueue.Models;

namespace SubscriberQueue.Handlers;

public class NewsletterEventHandler(SubscriberList subscribeList) : IMessageHandler<NewsletterEvent>
{
    public Task HandleAsync(NewsletterEvent message, CancellationToken ct)
    {
        var subs = subscribeList.GetList();
        List<Subscriber> activeSubs = subs.Where(s => s.isActive).ToList();

        foreach (var sub in activeSubs)
        {
            sub.RecieveMail(message.Content);
        }

        return Task.CompletedTask;
    }
}