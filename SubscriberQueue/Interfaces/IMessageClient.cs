using SubscriberQueue.Models;

namespace SubscriberQueue.Interfaces;

public interface IMessageClient
{
    Task Subscribe<T>(string subscriptionId, MessageHandler<T>? handler, CancellationToken token = default);
    Task Publish<T>(T message, CancellationToken token = default);
    Task Unsubscribe(string subscriptionId);
}
