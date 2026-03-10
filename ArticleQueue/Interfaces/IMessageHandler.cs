namespace ArticleQueue.Interfaces;

public interface IMessageHandler <TMessage>
{
    Task HandleAsync(TMessage message, CancellationToken ct);
}