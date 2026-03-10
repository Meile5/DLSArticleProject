using System;
using System.Collections.Generic;
using DraftService.Models;
using Microsoft.EntityFrameworkCore;

namespace DraftService.Database;

public partial class DraftContext : DbContext
{
    public DraftContext(DbContextOptions<DraftContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Draft> Drafts { get; set; }

    public virtual DbSet<DraftStatus> DraftStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Draft>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Drafts__3214EC07F5B6E780");

            entity.HasIndex(e => e.AuthorId, "IX_Drafts_AuthorId");

            entity.HasIndex(e => e.StatusId, "IX_Drafts_StatusId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AuthorId).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Status).WithMany(p => p.Drafts)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Drafts_Status");
        });

        modelBuilder.Entity<DraftStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DraftSta__3214EC0759E8903F");

            entity.ToTable("DraftStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
