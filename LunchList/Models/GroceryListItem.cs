namespace LunchList.Models
{
    public class GroceryListItem
    {
        public int GroceryListId { get; set; }
        public GroceryList GroceryList { get; set; }

        public int GroceryItemId { get; set; }
        public GroceryItem GroceryItem { get; set; }
    }
}