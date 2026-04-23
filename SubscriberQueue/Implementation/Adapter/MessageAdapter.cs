using EasyNetQ;
using SubscriberQueue.Interfaces;
using SubscriberQueue.Models;

namespace SubscriberQueue.Implementation.Adapter;

public class MessageAdapter(IBus _bus) : IMessageAdapter
{
    private Dictionary<string, SubscriptionResult> _subscriptions = new();

    public async Task Subscribe<T>(string subscriptionId, MessageHandler<T>? handler, CancellationToken token = default)
    {
        handler ??= new MessageHandler<T>(DefaultHandleTextMessage);
        var result = await _bus.PubSub.SubscribeAsync<T>(subscriptionId, handler.Handler, token);
        _subscriptions.Add(subscriptionId, result);
    }

    public async Task Publish<T>(T message, CancellationToken token = default)
    {
        Console.WriteLine($"Publishing message: {message}");
        await _bus.PubSub.PublishAsync(message, token);
        Console.WriteLine("Message sent!");
    }

    public Task Unsubscribe(string subscriptionId)
    {
        _subscriptions.Remove(subscriptionId);
        return Task.CompletedTask;
    }

    private static void DefaultHandleTextMessage<T>(T message)
    {
        Console.WriteLine(message != null ? "You got this message: " + message : "Default message");
    }
}
