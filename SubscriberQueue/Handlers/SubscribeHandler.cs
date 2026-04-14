using ArticleQueue.Interfaces;
using SubscriberQueue.Events;
using SubscriberQueue.Models;

namespace SubscriberQueue.Handlers;

public class SubscribeHandler(SubscriberList subscribeList) : IMessageHandler<NewSubscriberEvent>
{
    public Task HandleAsync(NewSubscriberEvent message, CancellationToken ct)
    {
        Subscriber newSub = new Subscriber
        {
            SubscriberId = message.SubscriberId,
            Username = message.Username,
            Email = message.Email,
            isActive = true
        };
        subscribeList.Subscribe(newSub);
        return Task.CompletedTask;
    }
}