namespace CommodityPriceManager.Web.Models;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class CommodityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int CategoryId { get; set; }
    public CategoryDto? Category { get; set; }
    public decimal CurrentPrice { get; set; }
}

public class PriceHistoryDto
{
    public int Id { get; set; }
    public int CommodityId { get; set; }
    public decimal Price { get; set; }
    public DateTime UpdatedAt { get; set; }
}
