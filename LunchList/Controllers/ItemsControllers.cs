using LunchList.Data;
using LunchList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunchList.Controllers
{
    public class GroceryListsController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to inject AppDbContext
        public GroceryListsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: GroceryLists
        public async Task<IActionResult> Index()
        {
            var groceryLists = await _context.GroceryLists
                .Include(g => g.GroceryListItems)  // Include related items
                .ThenInclude(gl => gl.GroceryItem)  // Include related grocery item
                .ToListAsync();

            return View(groceryLists);
        }

        // GET: GroceryLists/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var groceryList = await _context.GroceryLists
                .Include(g => g.GroceryListItems)
                .ThenInclude(gl => gl.GroceryItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (groceryList == null)
            {
                return NotFound();
            }

            return View(groceryList);
        }

        // GET: GroceryLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GroceryLists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Creater_at")] Grocery_lists groceryList)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Creater_at")] Grocery_lists groceryList)
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

