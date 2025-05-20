using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LunchList.Models;
using LunchList.Data;
using LunchList.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LunchList.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    // Constructor to inject AppDbContext
    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.RetailersProducts
            .ToListAsync();

        return View(products);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}