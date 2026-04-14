using ArticleQueue.Interfaces;
using Shared;
using SubscriberQueue.Events;
using SubscriberQueue.Models;

namespace SubscriberQueue.Handlers;

public class SubscribeHandler(SubscriberList subscribeList, IMessageClient client) : IMessageHandler<NewSubscriberEvent>
{
    public async Task HandleAsync(NewSubscriberEvent message, CancellationToken ct)
    {
        Subscriber newSub = new Subscriber
        {
            SubscriberId = message.SubscriberId,
            Username = message.Username,
            Email = message.Email,
            isActive = true
        };
        subscribeList.Subscribe(newSub);

        NewSubscriberSuccessEvent successEvent = NewSubscriberSuccessEvent.FromSubscriberEvent(message);

        await client.Publish(successEvent);
    }
}