using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunchList.Models
{
    public class GroceryItem
    {
        [Key]
        public int Id { get; set; }

        public int RetailerProductId { get; set; } // Foreign key

        public int Quantity { get; set; }

        public bool IsChecked { get; set; }

        public RetailerProduct RetailerProduct { get; set; } // Navigation property
    }
}
