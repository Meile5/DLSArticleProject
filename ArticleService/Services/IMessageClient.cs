namespace ArticleService.Services;

public interface IMessageClient
{
    Task PublishAsync<T>(T message) where T : class;
    Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> handler) where T : class;
}