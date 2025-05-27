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

            var selectedList = groceryLists.FirstOrDefault(gl => gl.Id == groceryListId);
            // Set ViewBag.IsDone to true if the selected list is marked as done (is_done == 1), false otherwise.
            ViewBag.IsDone = (selectedList != null && selectedList.Is_Done == 1);


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
        public async Task<IActionResult> SetDone(int? id)
        {

            return RedirectToAction("Index");
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

        [HttpPost]
        public async Task<IActionResult> CheckItem(int id)
        {
            // Execute a raw SQL update to set is_checked to 1 for the given item id.
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
              UPDATE grocery_items
              SET is_checked = 1
              WHERE id = {id}
                ");

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public async Task<IActionResult> FinishList(int id)
        //{
        //    // Execute a raw SQL update to set is_checked to 1 for the given item id.
        //    await _context.Database.ExecuteSqlInterpolatedAsync($@"
        //      UPDATE grocery_lists
        //      SET is_done = 1
        //      WHERE id = {id}
        //        ");

        //    return RedirectToAction("Index");
        //}
    }

}

     
