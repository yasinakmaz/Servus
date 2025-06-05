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
        public virtual DbSet<Price> ProductPrices { get; set; } = null!;
        public virtual DbSet<ProductFirstChoice> ProductFirstChoice { get; set; } = null!;
        public virtual DbSet<ProductSecondChoice> ProductSecondChoice { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureGlobalSettings(modelBuilder);

            ConfigureProductEntity(modelBuilder);
            ConfigureCategoryEntity(modelBuilder);

            ConfigureDatabaseSpecificSettings(modelBuilder);
        }

        private void ConfigureGlobalSettings(ModelBuilder modelBuilder)
        {
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
                .HasMaxLength(1000)
                .HasColumnType(_databaseProvider == DatabaseProvider.SqlServer ? "nvarchar(1000)" : "TEXT");

            productEntity.Property(p => p.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            productEntity.Property(p => p.Status)
                .IsRequired()
                .HasConversion<int>();

            productEntity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            productEntity.HasIndex(p => p.ProductName)
                .IsUnique()
                .HasDatabaseName("IX_Products_Name");

            productEntity.HasIndex(p => p.CategoryId)
                .HasDatabaseName("IX_Products_CategoryId");

            productEntity.HasIndex(p => p.Status)
                .HasDatabaseName("IX_Products_Status");

            productEntity.HasIndex(p => new { p.CategoryId, p.Status })
                .HasDatabaseName("IX_Products_Category_Status");
        }

        private void ConfigureCategoryEntity(ModelBuilder modelBuilder)
        {
            var categoryEntity = modelBuilder.Entity<ProductCategory>();

            categoryEntity.HasKey(p => p.Id);

            categoryEntity.HasIndex(c => c.CategoryName)
                .IsUnique()
                .HasDatabaseName("IX_Categories_Name");

            categoryEntity.HasIndex(c => c.DisplayOrder)
                .HasDatabaseName("IX_Categories_ParentId");
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
        }

        private void ConfigureSQLiteSettings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.CreatedDate)
                .HasDefaultValueSql("datetime('now')");

            modelBuilder.Entity<ProductCategory>()
                .Property(c => c.CreatedDate)
                .HasDefaultValueSql("datetime('now')");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.SetDeleteBehavior(DeleteBehavior.Restrict);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
#endif
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Product && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedDate").CurrentValue = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
