using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunchList.Models
{
    public class RetailerProduct
    {
        [Key]
        public int Id { get; set; }
        [Column("retailer_id")]
        public int? RetailerId { get; set; }
        [Column("name")]
        [MaxLength(100)]
        public string? Name { get; set; }
        [Column("price")]
        public decimal? Price { get; set; }

        public Retailer? Retailer { get; set; }
    }
}

