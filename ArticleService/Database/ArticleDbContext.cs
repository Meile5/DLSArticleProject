using ArticleService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArticleService.Database;

public class ArticleDbContext : DbContext
{
    public ArticleDbContext(DbContextOptions<ArticleDbContext> options)
        : base(options)
    {
    }

    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(a => a.ArticleId);
            entity.Property(a => a.Title).IsRequired();
            entity.Property(a => a.Contents).IsRequired();
            entity.Property(a => a.PublishingDate).IsRequired();
            entity.HasOne<Author>()
                  .WithMany()
                  .HasForeignKey(a => a.AuthorId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.AuthorId);
            entity.Property(a => a.AuthorName).IsRequired();
            entity.Property(a => a.AuthorLastName).IsRequired();
        });
    }
}
