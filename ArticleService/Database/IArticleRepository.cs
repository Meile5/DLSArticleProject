using ArticleService.Entities;

namespace ArticleService.Database;

public interface IArticleRepository
{
    // Optional shard parameter for z-axis sharding
    //later we can do repository.CreateAsync(article, "Continent1")
    Task<Article> CreateAsync(Article article, string shard = "Global");
    Task<Article?> GetByIdAsync(Guid articleId, string shard = "Global");
    Task<IEnumerable<Article>> GetAllAsync(string shard = "Global");
    Task UpdateAsync(Article article, string shard = "Global");
    Task DeleteAsync(Guid articleId, string shard = "Global");
}