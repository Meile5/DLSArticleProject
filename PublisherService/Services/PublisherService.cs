using ArticleQueue.Interfaces;
using ArticleQueue.Models.Events;
using PublisherService.Entities;

namespace PublisherService.Services;

public class PublisherService(IMessageClient _client)
{
    public async Task PublishArticleAsync(PublishArticleRequest request)
    {
        await _client.Publish(new ArticlePublishedEvent
        {
            ArticleId = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            AuthorId = request.AuthorId,
            PublishedAt = DateTime.UtcNow
        });
    }
}