using ArticleQueue.Interfaces;
using SubscriberQueue.Events;

namespace SubscriberQueue.Handlers;

public class SubscribeHandler : IMessageHandler<NewSubscriberEvent>
{
    public Task HandleAsync(NewSubscriberEvent message, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}