using ArticleQueue.Interfaces;
using ArticleQueue.Models.Events;
using NewsletterService.Entities;

namespace NewsletterService.Services;

public class NewsletterService(IMessageClient _client)
{
    public async Task NewsletterAsync (ArticlePublishedEvent request)
        {
            await _client.Publish(new NewsletterEvent
            {
                ArticleId = request.ArticleId,
                Title = request.Title,
                Content = request.Content,
                AuthorId = request.AuthorId,
                PublishedAt = request.PublishedAt
            });
        }
    }
