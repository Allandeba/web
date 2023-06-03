using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using getQuote.Models;
using System.Net.Mail;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using getQuote.DAO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace getQuote.Controllers;

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
        List<PersonModel> people = _context.Person.ToList(); 
        return View(people);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public Task<IActionResult> Create(PersonModel person)
    {
        if (!ModelState.IsValid) { return Task.FromResult<IActionResult>(View(person)); }

        _context.Person.Add(person);
        _context.SaveChanges();
        return Task.FromResult<IActionResult>(RedirectToAction(nameof(Index))); 
    }

    public IActionResult Update(int id)
    {
        PersonModel person = _context.Person.Include(c => c.Contact)
                                            .Include(d => d.Document)
                                            .FirstOrDefault(p => p.PersonId == id);
        return View(person);
    }

    [HttpPost]
    public Task<IActionResult> Update(PersonModel person)
    {
        if (!ModelState.IsValid) { return Task.FromResult<IActionResult>(View(person)); }

        PersonModel existentPerson = _context.Person.Include(c => c.Contact)
                                            .Include(d => d.Document)
                                            .FirstOrDefault(p => p.PersonId == person.PersonId);
        if (existentPerson != null)
        {
            // not working in the html hidden input.
            person.CreationDate = existentPerson.CreationDate;
            _context.Entry(existentPerson).CurrentValues.SetValues(person);
        }
        
        _context.SaveChanges();
        return Task.FromResult<IActionResult>(RedirectToAction(nameof(Index)));
    }

    public IActionResult Delete(int id)
    {
        PersonModel person = _context.Person.Include(c => c.Contact)
                                            .Include(d => d.Document)
                                            .FirstOrDefault(p => p.PersonId == id);
        _context.Person.Remove(person);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

