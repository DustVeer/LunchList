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
}