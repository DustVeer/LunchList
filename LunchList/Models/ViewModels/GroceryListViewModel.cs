namespace LunchList.Models;

public class GroceryListViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<GroceryListViewModelProducts> GroceryListViewModelProducts { get; set; } = new();
}

public class GroceryListViewModelProducts
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public decimal? Price { get; set; }
    public int Quantity { get; set; }
    public bool IsChecked { get; set; }
}