using ArticleQueue.Interfaces;
using ArticleQueue.Models.Events;
using MonitorService;

namespace NewsletterService.Handlers;

public class NewsletterHandler(Services.NewsletterService service): IMessageHandler<ArticlePublishedEvent>{

    public async Task HandleAsync(ArticlePublishedEvent message, CancellationToken ct)
    {
        using var activity = Monitoring.ActivitySource.StartActivity("Entered HandleAsync in NewsletterHandler");
        
        await service.NewsletterAsync(message);
    }
    
}