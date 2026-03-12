using ArticleQueue.Interfaces;
using ArticleQueue.Models;

namespace ArticleQueue.Implementation.Client;

public class MessageClient(IMessageAdapter adapter) : IMessageClient
{
    public async Task Subscribe<T>(string subscriptionId, MessageHandler<T>? handler = null, CancellationToken token = default)
    {
        await adapter.Subscribe(subscriptionId, handler, token );
    }

    public async Task Publish<T>(T message, CancellationToken token = default)
    {
        await adapter.Publish(message, token);
    }

    public async Task Unsubscribe(string subscriptionId)
    {
        await adapter.Unsubscribe(subscriptionId);
    }
}