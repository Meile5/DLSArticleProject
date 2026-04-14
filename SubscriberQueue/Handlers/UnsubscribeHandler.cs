using ArticleQueue.Interfaces;
using ArticleQueue.Models;
using Shared;
using Shared.Events;

namespace SubscriberQueue.Handlers;

public class UnsubscribeHandler(SubscriberList subscribeList) : IMessageHandler<SubscriberUnsubscribedEvent>
{
    public async Task HandleAsync(SubscriberUnsubscribedEvent message, CancellationToken ct)
    {
        subscribeList.Unsubscribe(message.Email);
        
    }
}