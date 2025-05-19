using Microsoft.EntityFrameworkCore;

namespace LunchList.DTO
{
    [Keyless]
    public class GroceryItemDTO
    {
        public int Id { get; set; }
        public int Retailer_Product_Id { get; set; }
        public int Quantity { get; set; }
        public bool Is_Checked_Bool { get; set; }
        public string RetailerProductName { get; set; }
    }
}
