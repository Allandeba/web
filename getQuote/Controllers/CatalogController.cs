using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using getQuote.Models;

namespace getQuote.Controllers
{
    public class CatalogController : Controller
    {
        private readonly CatalogBusiness _business;

        public CatalogController(CatalogBusiness catalogBusiness)
        {
            _business = catalogBusiness;
        }

        public IActionResult Index()
        {
            IEnumerable<ItemModel> items = _business.GetItems();
            return View(items);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(errorViewModel);
        }
    }
}
