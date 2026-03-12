using ArticleQueue.Interfaces;
using ArticleQueue.Models.Events;

namespace NewsletterService.Handlers;

public class NewsletterHandler(Services.NewsletterService service): IMessageHandler<ArticlePublishedEvent>{

    public async Task HandleAsync(ArticlePublishedEvent message, CancellationToken ct)
    {
        service.NewsletterAsync(message);
    }
    
}