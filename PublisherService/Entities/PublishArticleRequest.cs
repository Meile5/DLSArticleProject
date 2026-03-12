namespace PublisherService.Entities;

public class PublishArticleRequest
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required Guid AuthorId { get; set; }
}