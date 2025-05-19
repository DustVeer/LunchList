using LunchList.Data;
using LunchList.Models;
using LunchList.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LunchList.Controllers
{
    public class GroceryListController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to inject AppDbContext
        public GroceryListController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? selectedGroceryListId)
        {

            ViewBag.GroceryLists = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "List 1" },
                new SelectListItem { Value = "2", Text = "List 2" },
                new SelectListItem { Value = "3", Text = "List 3" }
            };

            // Optionally set a default value if none is provided.
            int groceryListId = selectedGroceryListId ?? 3;

            var groceryList = await _context.Set<GroceryItemDTO>()
                .FromSqlInterpolated($@"
                    SELECT 
                        gi.Id, 
                        gi.Retailer_Product_Id, 
                        gi.Quantity, 
                        CASE 
                            WHEN gi.Is_Checked = 1 THEN CAST(1 AS bit) 
                            ELSE CAST(0 AS bit) 
                        END AS Is_Checked_Bool,
                        rp.Name AS RetailerProductName
                    FROM grocery_items gi
                    INNER JOIN retailer_products rp ON gi.Retailer_Product_Id = rp.Id
                    WHERE gi.Id IN (
                        SELECT gli.grocery_item_id
                        FROM grocery_list_items gli
                        WHERE gli.grocery_list_id = {groceryListId}
                    )")
                .ToListAsync();

            // Pass the selected id to the view to maintain state in the dropdown.
            ViewBag.SelectedGroceryListId = groceryListId;

            return View(groceryList);
        }

        // GET: GroceryLists/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        // GET: GroceryLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GroceryLists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Creater_at")] GroceryList groceryList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groceryList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groceryList);
        }

        // GET: GroceryLists/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var groceryList = await _context.GroceryLists.FindAsync(id);
            if (groceryList == null)
            {
                return NotFound();
            }
            return View(groceryList);
        }

        // POST: GroceryLists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Creater_at")] GroceryList groceryList)
        {
            if (id != groceryList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groceryList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.GroceryLists.Any(e => e.Id == groceryList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(groceryList);
        }

        // GET: GroceryLists/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var groceryList = await _context.GroceryLists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groceryList == null)
            {
                return NotFound();
            }

            return View(groceryList);
        }

        // POST: GroceryLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groceryList = await _context.GroceryLists.FindAsync(id);
            _context.GroceryLists.Remove(groceryList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

