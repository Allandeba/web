﻿using Microsoft.AspNetCore.Mvc;
using getQuote.Models;

namespace getQuote.Controllers
{
    public class CatalogController : BaseController
    {
        private readonly CatalogBusiness _business;

        public CatalogController(CatalogBusiness catalogBusiness)
        {
            _business = catalogBusiness;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ItemModel> items = await _business.GetItems();
            return View(items);
        }
    }
}
