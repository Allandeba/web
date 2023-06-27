using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using getQuote.Models;
using getQuote.DAO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace getQuote.Controllers;

public class ProposalController : Controller
{
    private readonly ILogger<ProposalController> _logger;
    private readonly ApplicationDBContext _context;

    public ProposalController(ILogger<ProposalController> logger, ApplicationDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        List<ProposalModel> proposals = _context.Proposal.Include(a => a.Person).ToList();
        return View(proposals);
    }

    public IActionResult Create()
    {
        ViewBag.People = GetPeople();
        ViewBag.Items = GetItems();

        return View();
    }

    [HttpPost]
    public Task<IActionResult> Create(ProposalModel Proposal)
    {
        if (!ModelState.IsValid)
        {
            return Task.FromResult<IActionResult>(View(Proposal));
        }

        var person = _context.Person.Find(Proposal.ProposalPersonId);
        Proposal.Person = person;

        Proposal.ProposalContent = new();
        foreach (var itemId in Proposal.ItemIdList)
        {
            var item = _context.Item.Find(itemId);
            var proposalContent = new ProposalContentModel { Item = item };
            Proposal.ProposalContent.Add(proposalContent);
        }

        _context.Proposal.Add(Proposal);
        _context.SaveChanges();
        return Task.FromResult<IActionResult>(RedirectToAction(nameof(Index)));
    }

    public IActionResult Update(int id)
    {
        ProposalModel proposal = _context.Proposal
            .Include(p => p.Person)
            .Include(pc => pc.ProposalContent)
            .ThenInclude(pc => pc.Item)
            .FirstOrDefault(pp => pp.ProposalId == id);

        ViewBag.ProposalPerson = proposal.Person;
        ViewBag.ProposalContent = proposal.ProposalContent;
        ViewBag.People = GetPeople();
        ViewBag.Items = GetItems();

        return View(proposal);
    }

    [HttpPost]
    public Task<IActionResult> Update(ProposalModel proposal)
    {
        if (!ModelState.IsValid)
        {
            return Task.FromResult<IActionResult>(View(proposal));
        }

        var person = _context.Person.Find(proposal.ProposalPersonId);
        proposal.Person = person;

        proposal.ProposalContent = new();
        foreach (var itemId in proposal.ItemIdList)
        {
            var item = _context.Item.FirstOrDefault(i => i.ItemId == itemId);

            var proposalContent = new ProposalContentModel { Item = item };
            proposal.ProposalContent.Add(proposalContent);
        }

        ProposalModel existentProposal = _context.Proposal
            .Include(p => p.Person)
            .Include(pc => pc.ProposalContent)
            .FirstOrDefault(pp => pp.ProposalId == proposal.ProposalId);

        if (existentProposal != null)
        {
            _context.Entry(existentProposal).CurrentValues.SetValues(proposal);

            existentProposal.Person = proposal.Person;

            // Atualiza os ProposalContent existentes com base nos ItemIdList
            var existingProposalContentIds = existentProposal.ProposalContent
                .Select(pc => pc.Item.ItemId)
                .ToList();
            var newItemIds = proposal.ItemIdList.Except(existingProposalContentIds).ToList();

            foreach (var itemId in newItemIds)
            {
                var item = _context.Item.FirstOrDefault(i => i.ItemId == itemId);

                var proposalContent = new ProposalContentModel { Item = item };
                existentProposal.ProposalContent.Add(proposalContent);
            }
        }

        _context.SaveChanges();

        return Task.FromResult<IActionResult>(RedirectToAction(nameof(Index)));
    }

    [HttpPost]
    public IActionResult DeleteProposalContent(int id)
    {
        // Todo: caso manter dessa forma, mesmo que a pessoa não salve na view, o item será excluído
        var proposalContent = _context.ProposalContent.FirstOrDefault(
            pc => pc.ProposalContentId == id
        );

        if (proposalContent != null)
        {
            _context.ProposalContent.Remove(proposalContent);
            _context.SaveChanges();
        }

        return Ok();
    }

    public IActionResult Delete(int id)
    {
        ProposalModel proposal = _context.Proposal
            .Include(pc => pc.ProposalContent)
            .FirstOrDefault(p => p.ProposalId == id);
        _context.Proposal.Remove(proposal);

        foreach (var proposalContent in proposal.ProposalContent)
        {
            ProposalContentModel proposalContentModel = _context.ProposalContent.Find(
                proposalContent.ProposalContentId
            );
            _context.ProposalContent.Remove(proposalContentModel);
        }

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }

    public SelectList GetPeople() => new SelectList(_context.Person, "PersonId", "PersonName");

    private SelectList GetItems() => new SelectList(_context.Item, "ItemId", "ItemName");
}
