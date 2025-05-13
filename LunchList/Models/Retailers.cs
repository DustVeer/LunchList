using System.ComponentModel.DataAnnotations;

namespace LunchList.Models
{
    public class Retailers
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Retailers_products> RetailerProducts { get; set; }
    }
}
