using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunchList.Models
{
    public class Grocery_items
    {
        [Key]
        public int id { get; set; }

        public int retailer_product_id { get; set; } // Foreign key

        public int quantity { get; set; }

        public bool is_checked { get; set; }

        public Retailers_products RetailerProduct { get; set; } // Navigation property
    }
}
