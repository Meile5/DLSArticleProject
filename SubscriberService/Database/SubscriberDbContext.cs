using Microsoft.EntityFrameworkCore;
using SubscriberService.Entities;

namespace SubscriberService.Database;

public class SubscriberDbContext : DbContext
{
    public SubscriberDbContext(DbContextOptions<SubscriberDbContext> options)
        : base(options)
    {
    }

    public DbSet<Subscriber> Subscribers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscriber>(entity =>
        {
            entity.HasKey(s => s.SubscriberId);
            entity.Property(s => s.Email).IsRequired();
            entity.Property(s => s.SubscribedAt).IsRequired();
            entity.Property(s => s.IsActive).IsRequired();
        });
    }
}