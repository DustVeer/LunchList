using LunchList.Data;
using LunchList.Models;
using LunchList.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LunchList.Controllers;

public class HistoryController(AppDbContext context) : Controller
{
    private readonly AppDbContext _context = context;

    public async Task<IActionResult> Index()
    {
        var groceryLists = await _context.GroceryLists.OrderByDescending(gl => gl.Created_At).ToListAsync();
           
        return View(groceryLists);
    }

    public async Task<IActionResult> Details(int id)
    {
        GroceryList? groceryList = await _context.GroceryLists
            .FirstOrDefaultAsync(gl => gl.Id == id);

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
}