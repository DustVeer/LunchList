using System.Collections.Generic;

namespace LunchList.Models
{
    public class GroceryItem
    {
        public int Id { get; set; }

        public int Retailer_Product_Id { get; set; }

        public int Quantity { get; set; }

        public bool Is_Checked { get; set; } = false;

        public RetailerProduct RetailerProduct { get; set; }

        public ICollection<GroceryListItem> GroceryListItems { get; set; }
    }
}