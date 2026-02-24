namespace ArticleService.Entities;

public class Article
{
    public Guid ArticleId { get; set; } 
    public string Title { get; set; } = string.Empty;
    public string Contents { get; set; } = string.Empty;
    public DateTime PublishingDate { get; set; }
    public Guid AuthorId { get; set; } 
}