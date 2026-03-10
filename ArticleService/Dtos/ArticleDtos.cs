namespace ArticleService.Dtos
{
    public class ArticleCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Contents { get; set; } = string.Empty;
        public DateTime PublishingDate { get; set; }
        public Guid AuthorId { get; set; }
    }

    public class ArticleReadDto
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Contents { get; set; } = string.Empty;
        public DateTime PublishingDate { get; set; }
        public Guid AuthorId { get; set; }
    }

    public class ArticleUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Contents { get; set; } = string.Empty;
    }
}