using FilamentsAPI.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace FilamentsAPI.Persistence
{
    // In the project folder:
    // - dotnet ef migrations add MigrationName
    // - dotnet dotnet ef migrations script 

    /// <summary>
    /// Database context
    /// </summary>
    /// <remarks>Constructor</remarks>
    public class FilamentsAPIDbContext(IConfiguration configuration) : DbContext
    {
        /// <summary>
        /// Accounts
        /// </summary>
        public DbSet<AccountEntity> Accounts { get; set; }

        /// <summary>
        /// All ACLs
        /// </summary>
        public DbSet<AclEntity> ACLs { get; set; }

        /// <summary>
        /// Global settings.
        /// </summary>
        public DbSet<CsSetting> Settings { get; set; }

        /// <summary>
        /// Filaments.
        /// </summary>
        public DbSet<FilamentEntity> Filaments{ get; set; }

        /// <summary>
        /// Storage boxes.
        /// </summary>
        public DbSet<StorageboxEntity> Storageboxes { get; set; }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = configuration.GetConnectionString("FilamentsAPIDatabase");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Please configure the FilamentsAPIDatabase connection string.");
            }
            optionsBuilder.UseMySQL(connectionString);
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CsSetting>(entity =>
            {
                entity.HasKey(e => e.Name);
            });

            modelBuilder.Entity<AccountEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(32);
                entity.HasIndex(e => e.Username).IsUnique(true);
                entity.Property(e => e.SaltedPasswordHash).IsRequired().HasMaxLength(72); // 64 plus a hash
                entity.HasMany(e => e.ACLs).WithMany(a => a.Accounts);
            });

            modelBuilder.Entity<AclEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
                entity.HasIndex(e => e.Name).IsUnique(true);
                entity.HasMany(e => e.Accounts).WithMany(a => a.ACLs);
            });

            modelBuilder.Entity<FilamentEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne(e => e.StorageBox).WithMany(a => a.Filaments).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<StorageboxEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired();
                entity.HasMany(e => e.Filaments).WithOne(a => a.StorageBox).OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
