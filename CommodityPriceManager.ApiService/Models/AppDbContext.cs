using Microsoft.EntityFrameworkCore;

namespace CommodityPriceManager.ApiService.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Commodity> Commodities => Set<Commodity>();
    public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Gold" },
            new Category { Id = 2, Name = "Silver" },
            new Category { Id = 3, Name = "Oil" }
        );
        
        modelBuilder.Entity<Commodity>().HasData(
            new Commodity { Id = 1, Name = "Gold 9999", CategoryId = 1, CurrentPrice = 8000000 },
            new Commodity { Id = 2, Name = "Crude Oil WTI", CategoryId = 3, CurrentPrice = 1750000 }
        );
        
        modelBuilder.Entity<PriceHistory>().HasData(
            new PriceHistory { Id = 1, CommodityId = 1, Price = 7900000, UpdatedAt = DateTime.UtcNow.AddDays(-1) },
            new PriceHistory { Id = 2, CommodityId = 1, Price = 8000000, UpdatedAt = DateTime.UtcNow },
            new PriceHistory { Id = 3, CommodityId = 2, Price = 1750000, UpdatedAt = DateTime.UtcNow }
        );
    }
}
