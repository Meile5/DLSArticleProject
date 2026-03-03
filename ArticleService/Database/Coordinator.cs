using ArticleService.AppOptionsPattern;
using ArticleService.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace ArticleService.Database;

public class Coordinator
{
    private readonly AppOptions _options;

    public Coordinator(IOptions<AppOptions> options)
    {
        _options = options.Value;
    }

    public async Task<SqlConnection> GetConnectionAsync(Shard shard = Shard.Global)
    {
        var shardName = shard.ToString();

        if (!_options.ConnectionStrings.TryGetValue(shardName, out var connStr))
            throw new ArgumentException($"Shard '{shardName}' not configured.");

        var connection = new SqlConnection(connStr);
        await connection.OpenAsync();

        return connection;
    }
}