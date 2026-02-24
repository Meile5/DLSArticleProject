using ArticleService.Database;
using ArticleService.Dtos;
using ArticleService.Entities;

namespace ArticleService.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _repository;

    public ArticleService(IArticleRepository repository)
    {
        _repository = repository;
    }

    public async Task<ArticleReadDto> CreateArticleAsync(ArticleCreateDto dto, Shard shard = Shard.Global)
    {
        var article = new Article
        {
            ArticleId = Guid.NewGuid(),
            Title = dto.Title,
            Contents = dto.Contents,
            PublishingDate = dto.PublishingDate,
            AuthorId = dto.AuthorId
        };

        // Pass shard to repository if implemented
        var created = await _repository.CreateAsync(article, shard); 
        return MapToReadDto(created);
    }

    public async Task<ArticleReadDto?> GetArticleByIdAsync(Guid articleId, Shard shard = Shard.Global)
    {
        var article = await _repository.GetByIdAsync(articleId, shard);
        return article == null ? null : MapToReadDto(article);
    }

    public async Task<IEnumerable<ArticleReadDto>> GetAllArticlesAsync(Shard shard = Shard.Global)
    {
        var articles = await _repository.GetAllAsync(shard);
        return articles.Select(MapToReadDto);
    }

    public async Task<bool> UpdateArticleAsync(Guid articleId, ArticleUpdateDto dto, Shard shard = Shard.Global)
    {
        var existing = await _repository.GetByIdAsync(articleId, shard);
        if (existing == null)
            return false;

        existing.Title = dto.Title;
        existing.Contents = dto.Contents;

        await _repository.UpdateAsync(existing, shard);
        return true;
    }

    public async Task<bool> DeleteArticleAsync(Guid articleId, Shard shard = Shard.Global)
    {
        var existing = await _repository.GetByIdAsync(articleId, shard);
        if (existing == null) return false;

        await _repository.DeleteAsync(articleId, shard);
        return true;
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
