using System.ComponentModel.DataAnnotations;

namespace CommodityPriceManager.ApiService.Models;

public class Commodity
{
    public int Id { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public decimal CurrentPrice { get; set; }
    
    public ICollection<PriceHistory> PriceHistories { get; set; } = [];
}
