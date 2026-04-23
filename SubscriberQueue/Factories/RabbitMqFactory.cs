using EasyNetQ;
using SubscriberQueue.Configuration;
using SubscriberQueue.Implementation.Adapter;
using SubscriberQueue.Interfaces;

namespace SubscriberQueue.Factories;

public class RabbitMqFactory
{
    private static MessageAdapter CreateAdapter(MessageClientOptions options)
    {
        IBus bus = RabbitHutch.CreateBus(options.ConnectionString);
        return new MessageAdapter(bus);
    }

    public static IMessageClient CreateMessageClient(MessageClientOptions options)
    {
        MessageAdapter adapter = CreateAdapter(options);
        return new Implementation.Client.MessageClient(adapter);
    }
}
