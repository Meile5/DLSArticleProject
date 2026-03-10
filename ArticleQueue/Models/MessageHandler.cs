namespace ArticleQueue.Models;

public record MessageHandler<T> (Action<T> Handler);