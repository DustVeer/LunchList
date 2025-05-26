using System.ComponentModel.DataAnnotations.Schema;

namespace LunchList.Models;

public class GroceryListItem
{
    public int GroceryListId { get; set; }

    public int GroceryItemId { get; set; }

    public GroceryList GroceryList { get; set; }
    public GroceryItem GroceryItem { get; set; }
}