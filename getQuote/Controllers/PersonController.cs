using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using getQuote.Models;

namespace getQuote.Controllers
{
    public class PersonController : Controller
    {
        private readonly PersonBusiness _business;

        public PersonController(PersonBusiness business)
        {
            _business = business;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<PersonModel> people = await _business.GetPeople();
            return View(people);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonModel person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }

            await _business.AddAsync(person);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            PersonModel person = await _business.GetByIdAsync(id);
            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PersonModel person)
        {
            if (person == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return View(person);
            }

            await _business.UpdateAsync(person);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _business.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }
    }
}
