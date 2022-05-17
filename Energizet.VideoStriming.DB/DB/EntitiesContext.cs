using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Energizet.VideoStriming.DB.DB
{
    public partial class EntitiesContext : DbContext
    {
        public EntitiesContext()
        {
        }

        public EntitiesContext(DbContextOptions<EntitiesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Format> Formats { get; set; } = null!;
        public virtual DbSet<Quality> Qualitys { get; set; } = null!;
        public virtual DbSet<Video> Videos { get; set; } = null!;
        public virtual DbSet<VideoQuality> VideoQualitys { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException("Need to configure DbContext");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Format>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Format1)
                    .HasMaxLength(50)
                    .HasColumnName("Format");
            });

            modelBuilder.Entity<Quality>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Quality1).HasColumnName("Quality");
            });

            modelBuilder.Entity<Video>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Discription).HasMaxLength(2000);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Preview).HasColumnType("image");
            });

            modelBuilder.Entity<VideoQuality>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Format)
                    .WithMany(p => p.VideoQualities)
                    .HasForeignKey(d => d.FormatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VideoQualitys_Formats");

                entity.HasOne(d => d.Quality)
                    .WithMany(p => p.VideoQualities)
                    .HasForeignKey(d => d.QualityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VideoQualitys_Qualitys");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.VideoQualities)
                    .HasForeignKey(d => d.VideoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VideoQualitys_Videos");

                entity.Property(e => e.Size);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
