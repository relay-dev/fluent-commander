using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IntegrationTests.EntityFramework.SqlServer.Entities
{
    public partial class DatabaseCommanderContext : DbContext
    {
        public DatabaseCommanderContext()
        {
        }

        public DatabaseCommanderContext(DbContextOptions<DatabaseCommanderContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SampleTable> SampleTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SampleTable>(entity =>
            {
                entity.Property(e => e.SampleTableId).HasColumnName("SampleTableID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(suser_sname())");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SampleDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.SampleDecimal).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SampleUniqueIdentifier).HasDefaultValueSql("(newid())");

                entity.Property(e => e.SampleVarChar).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
