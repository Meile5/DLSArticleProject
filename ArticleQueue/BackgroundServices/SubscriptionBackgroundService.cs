using ArticleQueue.Interfaces;
using ArticleQueue.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ArticleQueue.BackgroundServices;

public class SubscriptionBackgroundService<TEvent> : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly IMessageClient _client;
    private readonly string _subscriptionId;

    public SubscriptionBackgroundService(
        IServiceProvider services, IMessageClient client, string subscriptionId)
    {
        _services = services;
        _client = client;
        _subscriptionId = subscriptionId;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await _client.Subscribe<TEvent>(
            _subscriptionId,
            new MessageHandler<TEvent>(async message =>
            {
                using var scope = _services.CreateScope();
                var handler = scope.ServiceProvider
                    .GetRequiredService<IMessageHandler<TEvent>>();
                await handler.HandleAsync(message, ct);
            }),
            ct
        );

        await Task.Delay(Timeout.Infinite, ct);
    }
}