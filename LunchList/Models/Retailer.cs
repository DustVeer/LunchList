using System.ComponentModel.DataAnnotations;

namespace LunchList.Models
{
    public class Retailer
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<RetailerProduct> RetailerProducts { get; set; }
    }
}
