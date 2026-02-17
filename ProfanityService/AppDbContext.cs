using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProfanityService.Entities;

namespace ProfanityService;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Word> Words { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.WordId).HasName("PK__Words__2C20F066E3356C38");

            entity.Property(e => e.WordId).HasMaxLength(50);
            entity.Property(e => e.Word1)
                .HasMaxLength(50)
                .HasColumnName("Word");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
