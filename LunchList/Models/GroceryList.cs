using Microsoft.EntityFrameworkCore;

namespace LunchList.Models
{
    public class GroceryList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Created_At { get; set; }


        public Byte Is_Done { get; set; }


        public List<GroceryItem> Items { get; set; }
        
        public ICollection<GroceryListItem> GroceryListItems { get; set; }

    }
}
