namespace ArticleQueue.Models.Events;

public class LatestNewsEvent
{
    public Guid ArticleId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime PublishedAt { get; set; }
}