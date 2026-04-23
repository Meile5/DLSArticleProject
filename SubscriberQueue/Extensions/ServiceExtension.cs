using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SubscriberQueue.BackgroundServices;
using SubscriberQueue.Configuration;
using SubscriberQueue.Factories;
using SubscriberQueue.Interfaces;

namespace SubscriberQueue.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddRabbitMqMessageClient(this IServiceCollection services, MessageClientOptions options)
    {
        IMessageClient messageClient = RabbitMqFactory.CreateMessageClient(options);
        services.AddSingleton(messageClient);
        return services;
    }

    public static IServiceCollection AddSubscription<TEvent>(this IServiceCollection services, string subscriptionId)
    {
        services.AddHostedService(provider =>
        {
            var client = provider.GetRequiredService<IMessageClient>();
            return new SubscriptionBackgroundService<TEvent>(provider, client, subscriptionId);
        });
        return services;
    }

    public static IServiceCollection AddMessagingHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerType = typeof(IMessageHandler<>);
        var handlers = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType))
            .ToList();

        foreach (var handler in handlers)
        {
            var interfaceType = handler.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType);
            services.AddScoped(interfaceType, handler);
        }

        return services;
    }
}
