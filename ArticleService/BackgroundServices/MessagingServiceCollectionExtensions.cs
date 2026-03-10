using ArticleService.Services;
using EasyNetQ;

namespace ArticleService.BackgroundServices;

public static class MessagingServiceCollectionExtensions
{
    public static IServiceCollection AddMessageClient(
        this IServiceCollection services,
        string connectionString)
    {
        // Register EasyNetQ and IBus
        services.AddEasyNetQ(connectionString);

        // Register our abstraction
        services.AddSingleton<IMessageClient, MessageClient>();

        return services;
    }
}