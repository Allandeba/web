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

    public async Task<IActionResult> Index()
    {
        List<ItemModel> items = await _context.Item.OrderByDescending(a => a.ItemId).ToListAsync();
        return View(items);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Create(ItemModel item)
    {
        if (item == null)
        {
            return BadRequest(ModelState);
        }

        item.SetItemImageList();
        item.SetDefaultImage(item.DefaultImage);

        if (!ModelState.IsValid)
        {
            return View(item);
        }

        _context.Item.Add(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Update(int id)
    {
        ItemModel item = _context.Item
            .Include(i => i.ItemImageList)
            .FirstOrDefault(i => i.ItemId == id);

        return View(item);
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Update(ItemModel item)
    {
        if (item == null)
        {
            return BadRequest(ModelState);
        }

        item.SetItemImageList();

        if (!ModelState.IsValid)
        {
            return View(item);
        }

        ItemModel existentItem = _context.Item
            .Include(i => i.ItemImageList)
            .FirstOrDefault(i => i.ItemId == item.ItemId);

        if (existentItem != null)
        {
            existentItem.ItemName = item.ItemName;
            existentItem.Value = item.Value;
            existentItem.Description = item.Description;

            if (item.ItemImageList != null)
            {
                existentItem.ItemImageList.AddRange(item.ItemImageList);
            }

            existentItem.SetDefaultImage(item.DefaultImage);
        }

        foreach (var idItemImage in item.IdImagesToDelete)
        {
            ItemImageModel itemImage = existentItem.ItemImageList.Find(
                im => im.ItemImageId == idItemImage
            );
            existentItem.ItemImageList.Remove(itemImage);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        ItemModel item = _context.Item
            .Include(i => i.ItemImageList)
            .FirstOrDefault(i => i.ItemId == id);

        if (item != null)
        {
            _context.Item.Remove(item);

            foreach (var image in item.ItemImageList)
            {
                ItemImageModel itemImage = _context.ItemImage.Find(image.ItemImageId);
                _context.ItemImage.Remove(itemImage);
            }

            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
