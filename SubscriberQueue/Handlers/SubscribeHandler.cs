using ArticleQueue.Interfaces;
using Shared;
using Shared.Events;
using SubscriberQueue.Events;
using SubscriberQueue.Models;

namespace SubscriberQueue.Handlers;

public class SubscribeHandler(SubscriberList subscribeList, IMessageClient client) : IMessageHandler<SubscriberCreatedEvent>
{
    public async Task HandleAsync(SubscriberCreatedEvent message, CancellationToken ct)
    {
        Subscriber newSub = new Subscriber
        {
            SubscriberId = message.SubscriberId,
            Email = message.Email,
            isActive = true
        };
        subscribeList.Subscribe(newSub);

        NewSubscriberSuccessEvent successEvent = NewSubscriberSuccessEvent.FromSubscriberEvent(message);

        await client.Publish(successEvent);
    }
}