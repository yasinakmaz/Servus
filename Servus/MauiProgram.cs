using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Servus.Data.Contexts;
using Servus.Models.Interfaces;
using Servus.Models.Enums;

namespace Servus
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Database Configuration
            ConfigureDatabase(builder);

            // Services Registration
            ConfigureServices(builder);

            // Pages Registration
            ConfigurePages(builder);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void ConfigureDatabase(MauiAppBuilder builder)
        {
            // SQLite için database path'i
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "servus.db");

            // DbContext'i kaydet
            builder.Services.AddDbContext<ProductDbContext>(options =>
            {
#if DEBUG
                // Debug modunda SQLite kullan
                options.UseSqlite($"Data Source={dbPath}")
                       .EnableSensitiveDataLogging()
                       .EnableDetailedErrors();
#else
                // Release modunda SQLite kullan (production'da SQL Server'a geçebilirsiniz)
                options.UseSqlite($"Data Source={dbPath}");
#endif
            });

            // DatabaseProvider'ı kaydet
            builder.Services.AddSingleton<DatabaseProvider>(_ => DatabaseProvider.SQLite);

            // DbContextFactory'yi kaydet
            builder.Services.AddSingleton<IDbContextFactory, DbContextFactory>();
        }

        private static void ConfigureServices(MauiAppBuilder builder)
        {
            // Repository'leri şimdilik interface'ler olarak kaydet
            // Implementasyonlar oluşturulana kadar comment out

            // builder.Services.AddScoped<IProductRepository, ProductRepository>();
            // builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            // builder.Services.AddScoped<IPriceRepository, PriceRepository>();
            // builder.Services.AddScoped<IProductFirstChoiceRepository, ProductFirstChoiceRepository>();
            // builder.Services.AddScoped<IProductSecondChoiceRepository, ProductSecondChoiceRepository>();

            // Unit of Work'ü kaydet
            // builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Handler'ları kaydet
            // builder.Services.AddScoped<IProductHandler, ProductHandler>();
            // builder.Services.AddScoped<ICategoryHandler, CategoryHandler>();
            // builder.Services.AddScoped<IPriceHandler, PriceHandler>();

            // Service'leri kaydet
            // builder.Services.AddScoped<IImageService, ImageService>();
            // builder.Services.AddScoped<IValidationService, ValidationService>();
            // builder.Services.AddSingleton<ILoggingService, LoggingService>();

            // Cache Service (Memory Cache kullanarak)
            builder.Services.AddMemoryCache();
            // builder.Services.AddScoped<ICacheService, MemoryCacheService>();
        }

        private static void ConfigurePages(MauiAppBuilder builder)
        {
            // MainPage'i kaydet
            builder.Services.AddSingleton<MainPage>();

            // Diğer sayfalar eklendiğinde buraya kaydedilecek
            // builder.Services.AddTransient<ProductListPage>();
            // builder.Services.AddTransient<ProductDetailPage>();
            // builder.Services.AddTransient<CategoryListPage>();
            // vb.
        }
    }

    // DbContextFactory implementasyonu
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DbContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ProductDbContext CreateContext(DatabaseProvider provider)
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "servus.db");

            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();

            if (provider == DatabaseProvider.SQLite)
            {
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
            else if (provider == DatabaseProvider.SqlServer)
            {
                // SQL Server connection string buraya eklenecek
                // optionsBuilder.UseSqlServer("Your SQL Server Connection String");
                throw new NotImplementedException("SQL Server provider henüz yapılandırılmamış");
            }

            return new ProductDbContext(optionsBuilder.Options, provider);
        }

        public ProductDbContext CreateContext()
        {
            return _serviceProvider.GetRequiredService<ProductDbContext>();
        }
    }
}