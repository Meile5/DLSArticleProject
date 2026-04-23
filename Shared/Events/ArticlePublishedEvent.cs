namespace Shared.Events;

public class ArticlePublishedEvent : Event
{
    public Guid ArticleId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string AuthorName { get; set; }
    public DateTime PublishedAt { get; set; }
}
