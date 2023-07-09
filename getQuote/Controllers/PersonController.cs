using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using getQuote.Models;
using getQuote.DAO;
using System.Threading.Tasks;
using System.Linq;

namespace getQuote.Controllers
{
    public class PersonController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly ApplicationDBContext _context;

        public PersonController(ILogger<PersonController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var people = _context.Person.OrderByDescending(a => a.PersonId).ToList();
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

            _context.Person.Add(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            var person = _context.Person
                .Include(c => c.Contact)
                .Include(d => d.Document)
                .FirstOrDefault(p => p.PersonId == id);
            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PersonModel person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }

            var existentPerson = _context.Person
                .Include(c => c.Contact)
                .Include(d => d.Document)
                .FirstOrDefault(p => p.PersonId == person.PersonId);

            if (existentPerson != null)
            {
                person.CreationDate = existentPerson.CreationDate;
                _context.Entry(existentPerson).CurrentValues.SetValues(person);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var person = _context.Person
                .Include(c => c.Contact)
                .Include(d => d.Document)
                .FirstOrDefault(p => p.PersonId == id);

            if (person != null)
            {
                _context.Person.Remove(person);
                await _context.SaveChangesAsync();
            }

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
