using LunchList.Data;
using LunchList.Models;
using LunchList.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LunchList.Controllers
{
    public class GroceryListController : Controller
    {
        private readonly AppDbContext _context;

        public GroceryListController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? selectedGroceryListId)
        {
            var groceryLists = await _context.GroceryLists.ToListAsync();

            int groceryListId = selectedGroceryListId ?? groceryLists.LastOrDefault()?.Id ?? 2;

            ViewBag.GroceryListSelect = groceryLists.Select(gl => new SelectListItem
            {
                Value = gl.Id.ToString(),
                Text = gl.Name
            }).ToList();

            var groceryItems = await _context.Set<GroceryItemDTO>()
                .FromSqlInterpolated($@"
            SELECT 
                gi.Id, 
                gi.Retailer_Product_Id, 
                gi.Quantity, 
                gi.is_checked,      
                rp.Name AS RetailerProductName
            FROM grocery_items gi
            INNER JOIN retailer_products rp ON gi.Retailer_Product_Id = rp.Id
            WHERE gi.Id IN (
                SELECT gli.grocery_item_id
                FROM grocery_list_items gli
                WHERE gli.grocery_list_id = {groceryListId}
            )")
                .ToListAsync();

            var viewModel = new HistoryViewModel
            {
                GroceryItems = groceryItems,
                GroceryLists = groceryLists
            };

            ViewBag.SelectedGroceryListId = groceryListId;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO grocery_lists (name) VALUES ({name});
        ");
            }
            return RedirectToAction("Index");
        }
    }
}

     
