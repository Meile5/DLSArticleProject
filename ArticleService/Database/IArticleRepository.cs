using ArticleService.Entities;

namespace ArticleService.Database;

public interface IArticleRepository
{
    Task<Article> CreateAsync(Article article);
    Task<Article?> GetByIdAsync(Guid articleId);
    Task<IEnumerable<Article>> GetAllAsync();
    Task UpdateAsync(Article article);
    Task DeleteAsync(Guid articleId);
}