using ArticleQueue.Interfaces;
using ArticleQueue.Models;
using Shared;

namespace SubscriberQueue.Handlers;

public class UnsubscribeHandler(SubscriberList subscribeList) : IMessageHandler<SubscriberRemovedEvent>
{
    public Task HandleAsync(SubscriberRemovedEvent message, CancellationToken ct)
    {
        subscribeList.Unsubscribe(message.SubscriberId);

        return Task.CompletedTask;
    }
}