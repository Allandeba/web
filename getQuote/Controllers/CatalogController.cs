using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using getQuote.Models;
using getQuote.DAO;
using Microsoft.EntityFrameworkCore;

namespace getQuote.Controllers;

public class CatalogController : Controller
{
    private readonly ILogger<CatalogController> _logger;
    private readonly ApplicationDBContext _context;

    public CatalogController(ILogger<CatalogController> logger, ApplicationDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        List<ItemModel> items = _context.Item.Include(i => i.ItemImageList).ToList();
        return View(items);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
