using Microsoft.EntityFrameworkCore;

namespace Servus.Services
{
    public interface IDatabaseService
    {
        Task InitializeDatabaseAsync();
        Task<bool> DatabaseExistsAsync();
        Task SeedInitialDataAsync();
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly ProductDbContext _context;

        public DatabaseService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task InitializeDatabaseAsync()
        {
            try
            {
                // Database'i oluştur veya güncelle
                await _context.Database.EnsureCreatedAsync();

                // Seed data'yı ekle
                await SeedInitialDataAsync();

                Console.WriteLine("✅ Database başarıyla oluşturuldu!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Database oluşturulurken hata: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DatabaseExistsAsync()
        {
            return await _context.Database.CanConnectAsync();
        }

        public async Task SeedInitialDataAsync()
        {
            // Eğer veri varsa seed yapma
            if (await _context.Categories.AnyAsync())
                return;

            // 1. Kategoriler oluştur
            var categories = new List<ProductCategory>
            {
                new ProductCategory
                {
                    CategoryName = "İçecekler",
                    Description = "Sıcak ve soğuk içecekler",
                    IsActive = true,
                    DisplayOrder = 1,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new ProductCategory
                {
                    CategoryName = "Kahve",
                    Description = "Kahve çeşitleri",
                    IsActive = true,
                    DisplayOrder = 2,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new ProductCategory
                {
                    CategoryName = "Tatlılar",
                    Description = "Tatlı çeşitleri",
                    IsActive = true,
                    DisplayOrder = 3,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                }
            };

            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            // 2. Fiyatlar oluştur
            var prices = new List<Price>
            {
                new Price
                {
                    PriceName = "Standart Fiyat",
                    PriceAmount = 25.00m,
                    Currency = (int)Currency.TRY,
                    IsActive = true,
                    IsDefault = true,
                    FirstChoice = false,
                    SecondChoice = false,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Price
                {
                    PriceName = "Küçük Boy",
                    PriceAmount = 20.00m,
                    Currency = (int)Currency.TRY,
                    IsActive = true,
                    IsDefault = false,
                    FirstChoice = true,
                    SecondChoice = false,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Price
                {
                    PriceName = "Büyük Boy",
                    PriceAmount = 30.00m,
                    Currency = (int)Currency.TRY,
                    IsActive = true,
                    IsDefault = false,
                    FirstChoice = true,
                    SecondChoice = false,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Price
                {
                    PriceName = "Şekerli",
                    PriceAmount = 2.00m,
                    Currency = (int)Currency.TRY,
                    IsActive = true,
                    IsDefault = false,
                    FirstChoice = false,
                    SecondChoice = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Price
                {
                    PriceName = "Şekersiz",
                    PriceAmount = 0.00m,
                    Currency = (int)Currency.TRY,
                    IsActive = true,
                    IsDefault = false,
                    FirstChoice = false,
                    SecondChoice = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                }
            };

            await _context.Prices.AddRangeAsync(prices);
            await _context.SaveChangesAsync();

            Console.WriteLine("✅ Seed data başarıyla eklendi!");
        }
    }
}