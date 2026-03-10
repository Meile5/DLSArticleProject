using ArticleService.Entities;

namespace ArticleService.Database;

public interface IArticleRepository
{
    // Optional shard parameter for z-axis sharding
    //later we can do repository.CreateAsync(article, "Continent1")
    Task<Article> CreateAsync(Article article, Shard shard = Shard.Global);
    Task<Article?> GetByIdAsync(Guid articleId, Shard shard = Shard.Global);
    Task<IEnumerable<Article>> GetAllAsync(Shard shard = Shard.Global);
    Task UpdateAsync(Article article, Shard shard = Shard.Global);
    Task DeleteAsync(Guid articleId, Shard shard = Shard.Global);
}