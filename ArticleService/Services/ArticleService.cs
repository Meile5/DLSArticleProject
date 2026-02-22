using ArticleService.Database;
using ArticleService.Dtos;
using ArticleService.Entities;
using ArticleService.Dtos;

namespace ArticleService.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _repository;

    public ArticleService(IArticleRepository repository)
    {
        _repository = repository;
    }

    public async Task<ArticleReadDto> CreateArticleAsync(ArticleCreateDto dto)
    {
        var article = new Article
        {
            ArticleId = Guid.NewGuid(),
            Title = dto.Title,
            Contents = dto.Contents,
            PublishingDate = dto.PublishingDate,
            AuthorId = dto.AuthorId
        };
        var created = await _repository.CreateAsync(article);
        return MapToReadDto(created);
    }

    public async Task<ArticleReadDto?> GetArticleByIdAsync(Guid articleId)
    {
        var article = await _repository.GetByIdAsync(articleId);
        return article == null ? null : MapToReadDto(article);
    }

    public async Task<IEnumerable<ArticleReadDto>> GetAllArticlesAsync()
    {
        var articles = await _repository.GetAllAsync();
        return articles.Select(MapToReadDto);
    }

    public async Task UpdateArticleAsync(Guid articleId, ArticleUpdateDto dto)
    {
        var existing = await _repository.GetByIdAsync(articleId);
        if (existing == null)
            throw new KeyNotFoundException("Article not found");

        existing.Title = dto.Title;
        existing.Contents = dto.Contents;

        await _repository.UpdateAsync(existing);
    }

    public async Task DeleteArticleAsync(Guid articleId)
    {
        await _repository.DeleteAsync(articleId);
    }

    private ArticleReadDto MapToReadDto(Article article) => new ArticleReadDto
    {
        ArticleId = article.ArticleId,
        Title = article.Title,
        Contents = article.Contents,
        PublishingDate = article.PublishingDate,
        AuthorId = article.AuthorId
    };
}