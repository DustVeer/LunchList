namespace LunchList.Models;

public class GroceryListViewModel
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<GroceryListViewModelProducts> GroceryListViewModelProducts { get; set; } = new();
    public decimal? TotalPrice { get; internal set; }
}

public class GroceryListViewModelProducts
{
    public GroceryItem GroceryItem { get; set; }
    public RetailerProduct RetailerProduct { get; set; }
    public decimal Price { get; set; }
    public string PriceFormatted => Price.ToString("0.00");
}