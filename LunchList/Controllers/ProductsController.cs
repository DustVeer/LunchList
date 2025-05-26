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

        
        var retailerProduct = await context.RetailersProducts.FindAsync(id);
        
        // Step 1: Create the grocery item
        var newItem = new GroceryItem
        {
            Quantity = 1,
            RetailerProduct = retailerProduct
        };

        context.GroceryItems.Add(newItem);
        await context.SaveChangesAsync(); // Save first to generate ID

        // Step 2: Create the link in GroceryListItems
        var listItem = new GroceryListItem
        {
            GroceryListId = 2,
            GroceryItemId = newItem.Id
        };

        context.GroceryListItems.Add(listItem);
        await context.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
    
   
}