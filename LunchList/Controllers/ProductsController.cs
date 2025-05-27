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

        // Step 1: Get the latest GroceryList ID using SQL
        var latestGroceryListId = await _context.GroceryLists
            .FromSqlInterpolated($"SELECT TOP 1 * FROM grocery_lists ORDER BY id DESC")
            .Select(gl => gl.Id)
            .FirstOrDefaultAsync();

        if (latestGroceryListId == 0)
        {
            return BadRequest("No grocery list found.");
        }

        // Step 2: Check if the item already exists in the grocery list using SQL
        var existingItemId = await _context.GroceryItems
            .FromSqlInterpolated($@"
            SELECT id FROM grocery_items 
            WHERE retailer_product_id = {id} 
            AND id IN (SELECT grocery_item_id FROM grocery_list_items WHERE grocery_list_id = {latestGroceryListId})
        ")
            .Select(gi => gi.Id)
            .FirstOrDefaultAsync();

        if (existingItemId > 0)
        {
            var isChecked = await _context.GroceryItems
                .FromSqlInterpolated($"SELECT TOP 1 is_checked FROM grocery_items WHERE id = {existingItemId}")
                .Select(gi => gi.Is_Checked)
                .FirstOrDefaultAsync();

            if (isChecked != 0)
            {
                // If existing item is checked, create a new item
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO grocery_items (retailer_product_id, is_checked, quantity)
                VALUES ({id}, {false}, {1});
            ");

                // Get the newest added GroceryItem ID
                var newItemId = await _context.GroceryItems
                    .FromSqlInterpolated($"SELECT TOP 1 id FROM grocery_items ORDER BY id DESC")
                    .Select(gi => gi.Id)
                    .FirstOrDefaultAsync();

                if (newItemId == 0)
                {
                    return BadRequest("Failed to retrieve new grocery item.");
                }

                // Link the newest GroceryItem to the latest GroceryList using SQL
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO grocery_list_items (grocery_list_id, grocery_item_id)
                VALUES ({latestGroceryListId}, {newItemId});
            ");
            }
            else
            {
                // If item exists and is NOT checked, increase the quantity
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE grocery_items 
                SET quantity = quantity + 1 
                WHERE id = {existingItemId};
            ");
            }
        }
        else
        {
            // If item does not exist, insert new item using SQL
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO grocery_items (retailer_product_id, is_checked, quantity)
            VALUES ({id}, {false}, {1});
        ");

            // Get the newest added GroceryItem ID using EF SQL
            var newItemId = await _context.GroceryItems
                .FromSqlInterpolated($"SELECT TOP 1 id FROM grocery_items ORDER BY id DESC")
                .Select(gi => gi.Id)
                .FirstOrDefaultAsync();

            if (newItemId == 0)
            {
                return BadRequest("Failed to retrieve new grocery item.");
            }

            // Link the newest GroceryItem to the latest GroceryList using SQL
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO grocery_list_items (grocery_list_id, grocery_item_id)
            VALUES ({latestGroceryListId}, {newItemId});
        ");
        }

        return RedirectToAction("Index");
    }
}