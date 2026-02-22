using ArticleService.Dtos;

namespace ArticleService.Services;

public interface IArticleService
{
    Task<ArticleReadDto> CreateArticleAsync(ArticleCreateDto dto, string shard = "Global");
    Task<ArticleReadDto?> GetArticleByIdAsync(Guid articleId, string shard = "Global");
    Task<IEnumerable<ArticleReadDto>> GetAllArticlesAsync(string shard = "Global");
    Task<bool> UpdateArticleAsync(Guid articleId, ArticleUpdateDto dto, string shard = "Global");
    Task<bool> DeleteArticleAsync(Guid articleId, string shard = "Global");
}