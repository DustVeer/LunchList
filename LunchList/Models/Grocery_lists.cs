using System.ComponentModel.DataAnnotations;

namespace LunchList.Models
{
    public class Grocery_lists
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string name { get; set; }

        public DateTime creater_at { get; set; }

        public ICollection<Grocery_list_items> GroceryListItems { get; set; }
    }
}
