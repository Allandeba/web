using System.Diagnostics;
using getQuote.DAO;
using getQuote.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ItemController : Controller
{
    private readonly ILogger<ItemController> _logger;
    private readonly ApplicationDBContext _context;

    public ItemController(ILogger<ItemController> logger, ApplicationDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        List<ItemModel> items = _context.Item.ToList();
        return View(items);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [
        DisableRequestSizeLimit,
        RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)
    ]
    public Task<IActionResult> Create(
        [Bind("ItemId, ItemName, Value, Description, ImageFiles, DefaultImage")] ItemModel Item
    )
    {
        if (Item == null)
        {
            return Task.FromResult<IActionResult>(BadRequest(ModelState));
        }

        Item.SetItemImageList();
        Item.SetDefaultImage(Item.DefaultImage);

        if (!ModelState.IsValid)
        {
            return Task.FromResult<IActionResult>(View(Item));
        }

        _context.Item.Add(Item);
        _context.SaveChanges();
        return Task.FromResult<IActionResult>(RedirectToAction(nameof(Index)));
    }

    public IActionResult Update(int id)
    {
        ItemModel Item = _context.Item
            .Include(i => i.ItemImageList)
            .FirstOrDefault(i => i.ItemId == id);

        return View(Item);
    }

    [HttpPost]
    [
        DisableRequestSizeLimit,
        RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)
    ]
    public Task<IActionResult> Update(ItemModel Item)
    {
        if (Item == null)
        {
            return Task.FromResult<IActionResult>(BadRequest(ModelState));
        }

        Item.SetItemImageList();

        if (!ModelState.IsValid)
        {
            return Task.FromResult<IActionResult>(View(Item));
        }

        ItemModel existentItem = _context.Item
            .Include(i => i.ItemImageList)
            .FirstOrDefault(i => i.ItemId == Item.ItemId);

        if (existentItem != null)
        {
            existentItem.ItemName = Item.ItemName;
            existentItem.Value = Item.Value;
            existentItem.Description = Item.Description;

            if (Item.ItemImageList != null)
            {
                foreach (var Image in Item.ItemImageList)
                {
                    existentItem.ItemImageList.Add(Image);
                }
            }

            existentItem.SetDefaultImage(Item.DefaultImage);
        }

        _context.SaveChanges();
        return Task.FromResult<IActionResult>(RedirectToAction(nameof(Index)));
    }

    public IActionResult Delete(int id)
    {
        ItemModel Item = _context.Item
            .Include(i => i.ItemImageList)
            .FirstOrDefault(i => i.ItemId == id);

        _context.Item.Remove(Item);

        foreach (var Image in Item.ItemImageList)
        {
            ItemImageModel ItemImage = _context.ItemImage.Find(Image.ItemImageId);
            _context.ItemImage.Remove(ItemImage);
        }

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult DeleteImage(int id)
    {
        ItemImageModel ItemImage = _context.ItemImage.FirstOrDefault(i => i.ItemImageId == id);
        _context.ItemImage.Remove(ItemImage);
        _context.SaveChanges();
        return RedirectToAction(nameof(Update), new { @id = ItemImage.ItemId });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new getQuote.Models.ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            }
        );
    }
}
