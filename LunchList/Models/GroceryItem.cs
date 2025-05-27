using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunchList.Models
{
    public class GroceryItem
    {
        [Key]
        public int Id { get; set; }

        public int Retailer_Product_Id { get; set; } // Foreign key

        public int Quantity { get; set; }

        public Byte Is_Checked { get; set; } = 0;

        public RetailerProduct RetailerProduct { get; set; } // Navigation property
        public ICollection<GroceryListItem> GroceryListItems { get; set; }
    }
}
