namespace LunchList.Models
{
    public class Grocery_list_items
    {
        public int grocery_list_id { get; set; }     // FK
        public int grocery_item_id { get; set; }     // FK

        public Grocery_lists GroceryList { get; set; }
        public Grocery_items GroceryItem { get; set; }
    }
}
