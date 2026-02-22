using System;
using System.Collections.Generic;
using CommentService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Database;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommentUser> CommentUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFCA68061A74");

            entity.Property(e => e.CommentId).HasMaxLength(50);
            entity.Property(e => e.ArticleId).HasMaxLength(50);
            entity.Property(e => e.Text).HasMaxLength(200);
        });

        modelBuilder.Entity<CommentUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CommentU__3214EC07AC759F46");

            entity.ToTable("CommentUser");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.CommentId).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(50);

            entity.HasOne(d => d.Comment).WithMany(p => p.CommentUsers)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CommentUs__Comme__4CA06362");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
