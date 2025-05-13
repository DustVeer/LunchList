using System.ComponentModel.DataAnnotations;

namespace LunchList.Models
{
    public class Retailers_products
    {
        [Key]
        public int Id { get; set; }

        public int retailer_id { get; set; } // FK

        [MaxLength(100)]
        public string name { get; set; }

        public int price { get; set; }

        public Retailers Retailer { get; set; }
    }
}

