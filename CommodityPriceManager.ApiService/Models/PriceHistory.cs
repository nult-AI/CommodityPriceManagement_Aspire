using System.ComponentModel.DataAnnotations;

namespace CommodityPriceManager.ApiService.Models;

public class PriceHistory
{
    public int Id { get; set; }
    
    public int CommodityId { get; set; }
    public Commodity? Commodity { get; set; }
    
    public decimal Price { get; set; }
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
