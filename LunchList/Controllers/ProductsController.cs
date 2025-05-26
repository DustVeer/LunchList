using System.Diagnostics;
using LunchList.Data;
using LunchList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LunchList.Models;

namespace LunchList.Controllers;

public class ProductsController(AppDbContext context) : Controller
{
    // Constructor to inject AppDbContext

    public async Task<IActionResult> Index()
    {
        var products = await context.RetailersProducts
            .ToListAsync();

        return View(products);
    }

    [HttpPost]
    public async Task<IActionResult> AddToList(int? id)
    {
        
        // 1. Create new grocery item
        var item = new GroceryItem
        {
            Retailer_Product_Id = id ?? 401,
            Quantity = 1,
            Is_Checked = false
        };

        context.GroceryItemEntities.Add(item);
        await context.SaveChangesAsync(); // must save to generate item.Id

        // 2. Add to an existing grocery list
        var listItem = new GroceryListItem
        {
            GroceryListId =  1, // Use default ID if null
            GroceryItemId = item.Id
        };

        context.GroceryListItems.Add(listItem);
        await context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
    
   
}