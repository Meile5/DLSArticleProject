using ArticleService.Entities;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace ArticleService.Database;

public class ArticleDatabase : IArticleRepository
{
    private readonly Coordinator _coordinator;

    public ArticleDatabase(Coordinator coordinator)
    {
        _coordinator = coordinator;
    }

    private SqlConnection GetGlobalConnection() => (SqlConnection)_coordinator.GetContinentGlobal();

    public async Task<Article> CreateAsync(Article article)
    {
        using var conn = GetGlobalConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Articles (ArticleId, Title, Contents, PublishingDate, AuthorId)
            VALUES (@ArticleId, @Title, @Contents, @PublishingDate, @AuthorId)";
        
        cmd.Parameters.AddWithValue("@ArticleId", article.ArticleId);
        cmd.Parameters.AddWithValue("@Title", article.Title);
        cmd.Parameters.AddWithValue("@Contents", article.Contents);
        cmd.Parameters.AddWithValue("@PublishingDate", article.PublishingDate);
        cmd.Parameters.AddWithValue("@AuthorId", article.AuthorId);

        await cmd.ExecuteNonQueryAsync();
        return article;
    }

    public async Task<Article?> GetByIdAsync(Guid articleId)
    {
        using var conn = GetGlobalConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Articles WHERE ArticleId = @ArticleId";
        cmd.Parameters.AddWithValue("@ArticleId", articleId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Article
            {
                ArticleId = reader.GetGuid(reader.GetOrdinal("ArticleId")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Contents = reader.GetString(reader.GetOrdinal("Contents")),
                PublishingDate = reader.GetDateTime(reader.GetOrdinal("PublishingDate")),
                AuthorId = reader.GetGuid(reader.GetOrdinal("AuthorId"))
            };
        }

        return null;
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        var articles = new List<Article>();
        using var conn = GetGlobalConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Articles";

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            articles.Add(new Article
            {
                ArticleId = reader.GetGuid(reader.GetOrdinal("ArticleId")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Contents = reader.GetString(reader.GetOrdinal("Contents")),
                PublishingDate = reader.GetDateTime(reader.GetOrdinal("PublishingDate")),
                AuthorId = reader.GetGuid(reader.GetOrdinal("AuthorId"))
            });
        }

        return articles;
    }

    public async Task UpdateAsync(Article article)
    {
        using var conn = GetGlobalConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE Articles
            SET Title = @Title, Contents = @Contents
            WHERE ArticleId = @ArticleId";

        cmd.Parameters.AddWithValue("@Title", article.Title);
        cmd.Parameters.AddWithValue("@Contents", article.Contents);
        cmd.Parameters.AddWithValue("@ArticleId", article.ArticleId);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(Guid articleId)
    {
        using var conn = GetGlobalConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Articles WHERE ArticleId = @ArticleId";
        cmd.Parameters.AddWithValue("@ArticleId", articleId);

        await cmd.ExecuteNonQueryAsync();
    }
}
