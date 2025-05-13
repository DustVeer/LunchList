using LunchList.Data;
using Microsoft.AspNetCore.Mvc;

namespace LunchList.Controllers
{
    public class GroceryListController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to inject DbContext
        public GroceryListController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /GroceryList
        public IActionResult Index()
        {
            var groceryItems = _context.GroceryItems.ToList();
            return View(groceryItems); 
        }
    }
}

