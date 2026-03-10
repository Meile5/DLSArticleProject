using EasyNetQ;

namespace ArticleService.Services;

public class MessageClient : IMessageClient
{
    private readonly IBus _bus;

    public MessageClient(IBus bus)
    {
        _bus = bus;
    }

    public Task PublishAsync<T>(T message) where T : class
    {
        return _bus.PubSub.PublishAsync(message);
    }

    public Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> handler) where T : class
    {
        return _bus.PubSub.SubscribeAsync(subscriptionId, handler);
    }
}