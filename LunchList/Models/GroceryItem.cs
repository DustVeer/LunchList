using System.Collections.Generic;

namespace LunchList.Models
{
    public class GroceryItem
    {
        public int Id { get; set; }

        public int Retailer_Product_Id { get; set; }

        public int Quantity { get; set; }

        public Byte Is_Checked { get; set; } = 0;

        public RetailerProduct RetailerProduct { get; set; }

        public ICollection<GroceryListItem> GroceryListItems { get; set; }
    }
}