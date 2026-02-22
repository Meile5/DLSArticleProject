using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace ArticleService.Database;

public class Coordinator
{
    // Map shard names to connection strings
    private readonly Dictionary<string, string> _connectionStrings = new()
    {
        { "Global", "Server=localhost,1433;Database=ArticleDb;User Id=sa;Password=SuperSecret7!;TrustServerCertificate=True;" },
        { "Continent1", "Server=localhost,1434;Database=ArticleDb;User Id=sa;Password=SuperSecret7!;TrustServerCertificate=True;" },
        { "Continent2", "Server=localhost,1435;Database=ArticleDb;User Id=sa;Password=SuperSecret7!;TrustServerCertificate=True;" },
        { "Continent3", "Server=localhost,1436;Database=ArticleDb;User Id=sa;Password=SuperSecret7!;TrustServerCertificate=True;" },
        { "Continent4", "Server=localhost,1437;Database=ArticleDb;User Id=sa;Password=SuperSecret7!;TrustServerCertificate=True;" },
        { "Continent5", "Server=localhost,1438;Database=ArticleDb;User Id=sa;Password=SuperSecret7!;TrustServerCertificate=True;" },
        { "Continent6", "Server=localhost,1439;Database=ArticleDb;User Id=sa;Password=SuperSecret7!;TrustServerCertificate=True;" },
        { "Continent7", "Server=localhost,1440;Database=ArticleDb;User Id=sa;Password=SuperSecret7!;TrustServerCertificate=True;" }
    };

    // Open a fresh connection for the requested shard
    public SqlConnection GetConnection(string shard = "Global")
    {
        if (!_connectionStrings.TryGetValue(shard, out var connStr))
            throw new ArgumentException($"Shard '{shard}' not found.");

        var conn = new SqlConnection(connStr);
        conn.Open(); // fresh connection, safe for multi-threading
        return conn;
    }
}