using System.Diagnostics;
using LunchList.Data;
using LunchList.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunchList.Controllers;

public class ProductsController : Controller
{
    // Constructor to inject AppDbContext
    private  AppDbContext _context;
    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.RetailersProducts
            .ToListAsync();

        return View(products);
    }

    [HttpPost]
    public async Task<IActionResult> AddToList(int? id)
    {
        if (id == null || id <= 0)
        {
            return BadRequest("Invalid Product ID.");
        }

        // Step 1: Get the latest GroceryList ID using SQL.
        var latestGroceryListId = await _context.GroceryLists
            .FromSqlInterpolated($"SELECT TOP 1 * FROM grocery_lists ORDER BY id DESC")
            .Select(gl => gl.Id)
            .FirstOrDefaultAsync();

        if (latestGroceryListId == 0)
        {
            return BadRequest("No grocery list found.");
        }

        // Step 2: Look for an unchecked item (is_checked = 0) for this product in the current grocery list.
        var uncheckedItem = await _context.GroceryItems
            .FromSqlInterpolated($@"
                SELECT TOP 1 *
                FROM grocery_items
                WHERE retailer_product_id = {id}
                  AND is_checked = {0}
                  AND id IN (
                      SELECT grocery_item_id 
                      FROM grocery_list_items 
                      WHERE grocery_list_id = {latestGroceryListId}
                  )
                ORDER BY id DESC
            ")
            .FirstOrDefaultAsync();

        if (uncheckedItem != null)
        {
            // An unchecked record exists; update its quantity.
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE grocery_items 
                SET quantity = quantity + 1 
                WHERE id = {uncheckedItem.Id};
            ");
        }
        else
        {
            // No unchecked record exists so insert a new item with quantity 1 and is_checked = 0.
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO grocery_items (retailer_product_id, is_checked, quantity)
                VALUES ({id}, {0}, {1});
            ");

            // Get the newly created GroceryItem Id.
            var newItemId = await _context.GroceryItems
                .FromSqlInterpolated($"SELECT TOP 1 id FROM grocery_items ORDER BY id DESC")
                .Select(gi => gi.Id)
                .FirstOrDefaultAsync();

            if (newItemId == 0)
            {
                return BadRequest("Failed to retrieve new grocery item.");
            }

            // Link the new grocery item to the current GroceryList.
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO grocery_list_items (grocery_list_id, grocery_item_id)
                VALUES ({latestGroceryListId}, {newItemId});
            ");
        }

        return RedirectToAction("Index","GroceryList");
    }

}