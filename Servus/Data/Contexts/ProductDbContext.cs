using Microsoft.EntityFrameworkCore;
using Servus.Models.Entities;
using Servus.Models.Enums;

namespace Servus.Data.Contexts
{
    public class ProductDbContext : DbContext
    {
        private readonly DatabaseProvider _databaseProvider;

        public ProductDbContext(DbContextOptions<ProductDbContext> options, DatabaseProvider databaseProvider = DatabaseProvider.SQLite)
            : base(options)
        {
            _databaseProvider = databaseProvider;
        }

        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> Categories { get; set; } = null!;
        public virtual DbSet<Price> Prices { get; set; } = null!;
        public virtual DbSet<ProductFirstChoice> ProductFirstChoices { get; set; } = null!;
        public virtual DbSet<ProductSecondChoice> ProductSecondChoices { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureGlobalSettings(modelBuilder);
            ConfigureProductEntity(modelBuilder);
            ConfigureCategoryEntity(modelBuilder);
            ConfigurePriceEntity(modelBuilder);
            ConfigureProductChoicesEntity(modelBuilder);
            ConfigureDatabaseSpecificSettings(modelBuilder);
        }

        private void ConfigureGlobalSettings(ModelBuilder modelBuilder)
        {
            // Decimal properties global configuration
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var decimalProperties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal) || p.PropertyType == typeof(decimal?));

                foreach (var property in decimalProperties)
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(property.Name)
                        .HasPrecision(18, 2);
                }
            }

            // String properties global configuration
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var stringProperties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(string));

                foreach (var property in stringProperties)
                {
                    if (!property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.StringLengthAttribute), false).Any())
                    {
                        modelBuilder.Entity(entityType.ClrType)
                            .Property(property.Name)
                            .HasMaxLength(255);
                    }
                }
            }
        }

        private void ConfigureProductEntity(ModelBuilder modelBuilder)
        {
            var productEntity = modelBuilder.Entity<Product>();

            productEntity.HasKey(p => p.IND);

            productEntity.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType(_databaseProvider == DatabaseProvider.SqlServer ? "nvarchar(200)" : "TEXT");

            productEntity.Property(p => p.ProductCode)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType(_databaseProvider == DatabaseProvider.SqlServer ? "nvarchar(50)" : "TEXT");

            productEntity.Property(p => p.Status)
                .IsRequired()
                .HasConversion<int>();

            // Foreign Key Relationships
            productEntity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            productEntity.HasOne(p => p.Price)
                .WithMany(pr => pr.Products)
                .HasForeignKey(p => p.PriceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            productEntity.HasIndex(p => p.ProductCode)
                .IsUnique()
                .HasDatabaseName("IX_Products_ProductCode");

            productEntity.HasIndex(p => p.ProductName)
                .HasDatabaseName("IX_Products_ProductName");

            productEntity.HasIndex(p => p.CategoryId)
                .HasDatabaseName("IX_Products_CategoryId");

            productEntity.HasIndex(p => p.PriceId)
                .HasDatabaseName("IX_Products_PriceId");

            productEntity.HasIndex(p => p.Status)
                .HasDatabaseName("IX_Products_Status");

            productEntity.HasIndex(p => new { p.CategoryId, p.Status })
                .HasDatabaseName("IX_Products_Category_Status");
        }

        private void ConfigureCategoryEntity(ModelBuilder modelBuilder)
        {
            var categoryEntity = modelBuilder.Entity<ProductCategory>();

            categoryEntity.HasKey(c => c.Id);

            categoryEntity.Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            categoryEntity.HasIndex(c => c.CategoryName)
                .IsUnique()
                .HasDatabaseName("IX_Categories_CategoryName");

            categoryEntity.HasIndex(c => c.DisplayOrder)
                .HasDatabaseName("IX_Categories_DisplayOrder");

            categoryEntity.HasIndex(c => c.IsActive)
                .HasDatabaseName("IX_Categories_IsActive");
        }

        private void ConfigurePriceEntity(ModelBuilder modelBuilder)
        {
            var priceEntity = modelBuilder.Entity<Price>();

            priceEntity.HasKey(p => p.IND);

            priceEntity.Property(p => p.PriceName)
                .IsRequired()
                .HasMaxLength(100);

            priceEntity.Property(p => p.PriceAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            priceEntity.HasIndex(p => p.IsActive)
                .HasDatabaseName("IX_Prices_IsActive");

            priceEntity.HasIndex(p => p.IsDefault)
                .HasDatabaseName("IX_Prices_IsDefault");

            priceEntity.HasIndex(p => p.Currency)
                .HasDatabaseName("IX_Prices_Currency");
        }

        private void ConfigureProductChoicesEntity(ModelBuilder modelBuilder)
        {
            // ProductFirstChoice Configuration
            var firstChoiceEntity = modelBuilder.Entity<ProductFirstChoice>();

            firstChoiceEntity.HasKey(fc => fc.IND);

            firstChoiceEntity.Property(fc => fc.ChoiceName)
                .IsRequired()
                .HasMaxLength(100);

            firstChoiceEntity.HasOne(fc => fc.Product)
                .WithMany(p => p.FirstChoices)
                .HasForeignKey(fc => fc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            firstChoiceEntity.HasOne(fc => fc.ProductPrice)
                .WithMany(pp => pp.FirstChoicePrices)
                .HasForeignKey(fc => fc.ProductPriceId)
                .OnDelete(DeleteBehavior.Restrict);

            firstChoiceEntity.HasIndex(fc => fc.ProductId)
                .HasDatabaseName("IX_ProductFirstChoices_ProductId");

            firstChoiceEntity.HasIndex(fc => fc.ProductPriceId)
                .HasDatabaseName("IX_ProductFirstChoices_ProductPriceId");

            // ProductSecondChoice Configuration
            var secondChoiceEntity = modelBuilder.Entity<ProductSecondChoice>();

            secondChoiceEntity.HasKey(sc => sc.IND);

            secondChoiceEntity.Property(sc => sc.ChoiceName)
                .IsRequired()
                .HasMaxLength(100);

            secondChoiceEntity.HasOne(sc => sc.Product)
                .WithMany(p => p.SecondChoices)
                .HasForeignKey(sc => sc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            secondChoiceEntity.HasOne(sc => sc.ProductPrice)
                .WithMany(pp => pp.SecondChoicePrices)
                .HasForeignKey(sc => sc.ProductPriceId)
                .OnDelete(DeleteBehavior.Restrict);

            secondChoiceEntity.HasIndex(sc => sc.ProductId)
                .HasDatabaseName("IX_ProductSecondChoices_ProductId");

            secondChoiceEntity.HasIndex(sc => sc.ProductPriceId)
                .HasDatabaseName("IX_ProductSecondChoices_ProductPriceId");
        }

        private void ConfigureDatabaseSpecificSettings(ModelBuilder modelBuilder)
        {
            if (_databaseProvider == DatabaseProvider.SqlServer)
            {
                ConfigureSqlServerSettings(modelBuilder);
            }
            else if (_databaseProvider == DatabaseProvider.SQLite)
            {
                ConfigureSQLiteSettings(modelBuilder);
            }
        }

        private void ConfigureSqlServerSettings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ProductCategory>()
                .Property(c => c.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Price>()
                .Property(p => p.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ProductFirstChoice>()
                .Property(pfc => pfc.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ProductSecondChoice>()
                .Property(psc => psc.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");
        }

        private void ConfigureSQLiteSettings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.CreatedDate)
                .HasDefaultValueSql("datetime('now')");

            modelBuilder.Entity<ProductCategory>()
                .Property(c => c.CreatedDate)
                .HasDefaultValueSql("datetime('now')");

            modelBuilder.Entity<Price>()
                .Property(p => p.CreatedDate)
                .HasDefaultValueSql("datetime('now')");

            modelBuilder.Entity<ProductFirstChoice>()
                .Property(pfc => pfc.CreatedDate)
                .HasDefaultValueSql("datetime('now')");

            modelBuilder.Entity<ProductSecondChoice>()
                .Property(psc => psc.CreatedDate)
                .HasDefaultValueSql("datetime('now')");

            // SQLite'da foreign key constraints'i aktif etmek için
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                if (relationship.DeleteBehavior == DeleteBehavior.Cascade)
                    continue; // Cascade olanları değiştirme

                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // UseLazyLoadingProxies için ayrı paket gerekiyor, şimdilik comment out
            // optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
#endif
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => (e.Entity is Product || e.Entity is ProductCategory ||
                           e.Entity is Price || e.Entity is ProductFirstChoice ||
                           e.Entity is ProductSecondChoice) &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    // UpdatedDate property'sini güncelle
                    var updatedDateProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "UpdatedDate");
                    if (updatedDateProperty != null)
                    {
                        updatedDateProperty.CurrentValue = DateTime.UtcNow;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}