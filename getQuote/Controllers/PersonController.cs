using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using getQuote.Models;
using System.Net.Mail;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using getQuote.DAO;
using System.Collections.Generic;

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

    [HttpPost]
    public IActionResult Create(PersonModel person)
    {
        if (!ModelState.IsValid) { return View(); }

        throw new ApplicationException("Not implemented yet");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

