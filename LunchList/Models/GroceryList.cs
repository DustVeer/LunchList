using System.ComponentModel.DataAnnotations;

namespace LunchList.Models
{
    public class GroceryList
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<GroceryItem> Items { get; set; }
    }
}
