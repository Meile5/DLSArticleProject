namespace SubscriberQueue.Models;

public record MessageHandler<T>(Action<T> Handler);
