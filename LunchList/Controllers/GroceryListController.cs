using LunchList.Data;
using LunchList.Models;
using LunchList.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Numerics;

namespace LunchList.Controllers
{
    public class GroceryListController : Controller
    {
        private readonly AppDbContext _context;

        public GroceryListController(AppDbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            
            GroceryList? groceryList = await _context.GroceryLists
                .FirstOrDefaultAsync(gl => gl.Is_Done == 0);

            if (groceryList == null)
            {
                TempData["ShowModal"] = true;
                return View(new GroceryListViewModel());
            }
            
            
            
            var products = await _context.GroceryListItems
                .Where(gli => gli.GroceryListId == groceryList.Id)
                .Include(gli => gli.GroceryItem)
                .ThenInclude(gi => gi.RetailerProduct)
                .Select(gli => new GroceryListViewModelProducts
                {
                    GroceryItem = gli.GroceryItem,
                    RetailerProduct = gli.GroceryItem.RetailerProduct,
                    Price = gli.GroceryItem.RetailerProduct.PricePerProduct * gli.GroceryItem.Quantity
                })
                .ToListAsync();
            
            var totalListPrice = products.Sum(item => item.GroceryItem.RetailerProduct.PricePerProduct * item.GroceryItem.Quantity);

            var model = new GroceryListViewModel()
            {
                Id = groceryList.Id,
                Name = groceryList.Name,
                GroceryListViewModelProducts = products,
                TotalPrice = totalListPrice
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SetDone(int? id)
        {
            var groceryList = await _context.GroceryLists.FindAsync(id);
            if (groceryList == null)
            {
                return NotFound();
            }

            groceryList.Is_Done = 1; 

            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(int? id)
        {
            var listItem = await _context.GroceryListItems
                .Include(gli => gli.GroceryItem)
                .FirstOrDefaultAsync(gli => gli.GroceryItemId == id);

            if (listItem == null)
            {
                return NotFound();
            }
           
            _context.GroceryItems.Remove(listItem.GroceryItem);
            _context.GroceryListItems.Remove(listItem);

            await _context.SaveChangesAsync();
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

     
