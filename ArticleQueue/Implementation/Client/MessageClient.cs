using ArticleQueue.Interfaces;
using ArticleQueue.Models;
using MonitorService;

namespace ArticleQueue.Implementation.Client;

public class MessageClient(IMessageAdapter adapter) : IMessageClient
{
    public async Task Subscribe<T>(string subscriptionId, MessageHandler<T>? handler = null, CancellationToken token = default)
    {
        //using var activity = Monitoring.ActivitySource.StartActivity("Subscribe (type: "+ typeof(T).FullName +") called in MessageClient");
        
        await adapter.Subscribe(subscriptionId, handler, token );
    }

    public async Task Publish<T>(T message, CancellationToken token = default)
    {
        using var activity = Monitoring.ActivitySource.StartActivity("Publish (type: "+ typeof(T).FullName +") called in MessageClient");
        
        await adapter.Publish(message, token);
    }

    public async Task Unsubscribe(string subscriptionId)
    {
        using var activity = Monitoring.ActivitySource.StartActivity("Unsubscribe called in MessageClient");
        
        await adapter.Unsubscribe(subscriptionId);
    }
}