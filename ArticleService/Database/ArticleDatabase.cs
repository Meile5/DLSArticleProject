using ArticleService.Entities;
using Microsoft.Data.SqlClient;

namespace ArticleService.Database;

public class ArticleDatabase : IArticleRepository
{
    private readonly Coordinator _coordinator;

    public ArticleDatabase(Coordinator coordinator)
    {
        _coordinator = coordinator;
    }

    public async Task<Article> CreateAsync(Article article, Shard shard = Shard.Global)
    {
        await using var conn = await _coordinator.GetConnectionAsync(shard);
        await using var cmd = conn.CreateCommand();

        cmd.CommandText = @"
            INSERT INTO Articles (ArticleId, Title, Contents, PublishingDate, AuthorName)
            VALUES (@ArticleId, @Title, @Contents, @PublishingDate, @AuthorName)";

        cmd.Parameters.AddWithValue("@ArticleId", article.ArticleId);
        cmd.Parameters.AddWithValue("@Title", article.Title);
        cmd.Parameters.AddWithValue("@Contents", article.Contents);
        cmd.Parameters.AddWithValue("@PublishingDate", article.PublishingDate);
        cmd.Parameters.AddWithValue("@AuthorName", article.AuthorName);

        await cmd.ExecuteNonQueryAsync();
        return article;
    }

    public async Task<Article?> GetByIdAsync(Guid articleId, Shard shard = Shard.Global)
    {
        await using var conn = await _coordinator.GetConnectionAsync(shard);
        await using var cmd = conn.CreateCommand();

        cmd.CommandText = "SELECT * FROM Articles WHERE ArticleId = @ArticleId";
        cmd.Parameters.AddWithValue("@ArticleId", articleId);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Article
            {
                ArticleId = reader.GetGuid(reader.GetOrdinal("ArticleId")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Contents = reader.GetString(reader.GetOrdinal("Contents")),
                PublishingDate = reader.GetDateTime(reader.GetOrdinal("PublishingDate")),
                AuthorName = reader.GetString(reader.GetOrdinal("AuthorName"))
            };
        }

        return null;
    }

    public async Task<IEnumerable<Article>> GetAllAsync(Shard shard = Shard.Global)
    {
        var articles = new List<Article>();

        await using var conn = await _coordinator.GetConnectionAsync(shard);
        await using var cmd = conn.CreateCommand();

        cmd.CommandText = "SELECT * FROM Articles";

        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            articles.Add(new Article
            {
                ArticleId = reader.GetGuid(reader.GetOrdinal("ArticleId")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Contents = reader.GetString(reader.GetOrdinal("Contents")),
                PublishingDate = reader.GetDateTime(reader.GetOrdinal("PublishingDate")),
                AuthorName = reader.GetString(reader.GetOrdinal("AuthorName"))
            });
        }

        return articles;
    }

    public async Task UpdateAsync(Article article, Shard shard = Shard.Global)
    {
        await using var conn = await _coordinator.GetConnectionAsync(shard);
        await using var cmd = conn.CreateCommand();

        cmd.CommandText = @"
            UPDATE Articles
            SET Title = @Title,
                Contents = @Contents,
                PublishingDate = @PublishingDate,
                AuthorName = @AuthorName
            WHERE ArticleId = @ArticleId";

        cmd.Parameters.AddWithValue("@Title", article.Title);
        cmd.Parameters.AddWithValue("@Contents", article.Contents);
        cmd.Parameters.AddWithValue("@PublishingDate", article.PublishingDate);
        cmd.Parameters.AddWithValue("@AuthorName", article.AuthorName);
        cmd.Parameters.AddWithValue("@ArticleId", article.ArticleId);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(Guid articleId, Shard shard = Shard.Global)
    {
        await using var conn = await _coordinator.GetConnectionAsync(shard);
        await using var cmd = conn.CreateCommand();

        cmd.CommandText = "DELETE FROM Articles WHERE ArticleId = @ArticleId";
        cmd.Parameters.AddWithValue("@ArticleName", articleId);

        await cmd.ExecuteNonQueryAsync();
    }
}