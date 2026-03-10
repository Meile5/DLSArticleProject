using ArticleQueue.Models;

namespace ArticleQueue.Interfaces;

public interface IMessageAdapter
{
    public Task Subscribe<T>(string subscriptionId, MessageHandler<T>? handler,
        CancellationToken token = default);

    public Task Publish<T>(T message, CancellationToken token = default);
    public Task Unsubscribe(string subscriptionId);
}