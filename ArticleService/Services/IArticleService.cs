using ArticleService.Dtos;
using ArticleService.Entities;

namespace ArticleService.Services;

public interface IArticleService
{
    Task<ArticleReadDto> CreateArticleAsync(ArticleCreateDto dto, Shard shard = Shard.Global);
    Task<ArticleReadDto?> GetArticleByIdAsync(Guid articleId, Shard shard = Shard.Global);
    Task<IEnumerable<ArticleReadDto>> GetAllArticlesAsync(Shard shard = Shard.Global);
    Task<bool> UpdateArticleAsync(Guid articleId, ArticleUpdateDto dto, Shard shard = Shard.Global);
    Task<bool> DeleteArticleAsync(Guid articleId, Shard shard = Shard.Global);
}