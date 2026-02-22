using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Data.SqlClient;

namespace ArticleService.Database;

public class ArticleDbContextFactory : IDesignTimeDbContextFactory<ArticleDbContext>
{
    public ArticleDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ArticleDbContext>();

        // Use Global DB connection string for design-time
        var connString = "Server=localhost,1433;Database=ArticleDb;User Id=sa;Password=SuperSecret7!;Encrypt=false;";
        optionsBuilder.UseSqlServer(connString);

        return new ArticleDbContext(optionsBuilder.Options);
    }
}