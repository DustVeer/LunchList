using LunchList.DTO;

namespace LunchList.Models
{
    public class HistoryViewModel
    {
        public List<GroceryItemDTO> GroceryItems { get; set; }
        public List<GroceryList> GroceryLists { get; set; }
        public List<RetailerProduct> RetailerProducts { get; set; }
    }
}
