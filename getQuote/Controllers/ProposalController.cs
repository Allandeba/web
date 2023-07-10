using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using getQuote.Models;
using getQuote.DAO;

namespace getQuote.Controllers
{
    public class ProposalController : Controller
    {
        private readonly ILogger<ProposalController> _logger;
        private readonly ApplicationDBContext _context;

        public ProposalController(ILogger<ProposalController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var proposals = await _context.Proposal
                .OrderByDescending(a => a.ProposalId)
                .Include(a => a.Person)
                .ToListAsync();

            return View(proposals);
        }

        public IActionResult Create()
        {
            PopulateViewBagDefault();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProposalModel proposal)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBagDefault();
                return View(proposal);
            }

            var person = await _context.Person.FindAsync(proposal.PersonId);
            proposal.Person = person;

            foreach (var proposalContent in proposal.ProposalContent)
            {
                var item = await _context.Item.FirstOrDefaultAsync(
                    i => i.ItemId == proposalContent.ItemId
                );
                proposalContent.Item = item;
            }

            _context.Proposal.Add(proposal);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var proposal = await _context.Proposal
                .Include(p => p.Person)
                .Include(pc => pc.ProposalContent)
                .ThenInclude(pc => pc.Item)
                .FirstOrDefaultAsync(pp => pp.ProposalId == id);

            if (proposal == null)
            {
                return NotFound();
            }

            PopulateViewBagUpdate(proposal);

            return View(proposal);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProposalModel proposal)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBagUpdate(proposal);
                return View(proposal);
            }

            var person = await _context.Person.FindAsync(proposal.PersonId);
            proposal.Person = person;

            foreach (var proposalContent in proposal.ProposalContent)
            {
                var item = await _context.Item.FirstOrDefaultAsync(
                    i => i.ItemId == proposalContent.ItemId
                );
                proposalContent.Item = item;
            }

            var existentProposal = await _context.Proposal
                .Include(p => p.Person)
                .Include(pc => pc.ProposalContent)
                .FirstOrDefaultAsync(pp => pp.ProposalId == proposal.ProposalId);

            if (existentProposal == null)
            {
                return NotFound();
            }

            setProposalHistoryAsync(existentProposal);

            // Update existent Proposal
            _context.Entry(existentProposal).CurrentValues.SetValues(proposal);
            existentProposal.Person = proposal.Person;

            // Remove deleted proposals
            var updatedProposalContent = proposal.ProposalContent
                .Select(pc => pc.ProposalContentId)
                .ToList();
            List<int> updatedProposalContentIds = proposal.ProposalContent
                .Select(pc => pc.ProposalContentId)
                .ToList();
            var existentProposalContent = _context.ProposalContent
                .Where(
                    pc =>
                        !updatedProposalContentIds.Contains(pc.ProposalContentId)
                        && pc.ProposalId == proposal.ProposalId
                )
                .ToList();
            foreach (var ExistentProposalContent in existentProposalContent)
            {
                existentProposal.ProposalContent.Remove(ExistentProposalContent);
            }

            foreach (var proposalContent in proposal.ProposalContent)
            {
                var updateProposalContent = existentProposal.ProposalContent.Find(
                    a => a.Item.ItemId == proposalContent.Item.ItemId
                );
                if (updateProposalContent == null)
                {
                    existentProposal.ProposalContent.Add(proposalContent);
                }
                else
                {
                    updateProposalContent = proposalContent;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var proposal = await _context.Proposal
                .Include(pc => pc.ProposalContent)
                .FirstOrDefaultAsync(p => p.ProposalId == id);

            if (proposal == null)
            {
                return NotFound();
            }

            _context.Proposal.Remove(proposal);

            foreach (var proposalContent in proposal.ProposalContent)
            {
                var proposalContentModel = await _context.ProposalContent.FindAsync(
                    proposalContent.ProposalContentId
                );
                _context.ProposalContent.Remove(proposalContentModel);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Print(int id)
        {
            var proposal = GetProposal(id);
            return View(proposal);
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

        public SelectList GetSelectListPeople() =>
            new SelectList(_context.Person, "PersonId", "PersonName");

        private SelectList GetSelectListItems() =>
            new SelectList(_context.Item, "ItemId", "ItemName");

        private List<dynamic> GetItemList()
        {
            return _context.Item
                .Select(
                    i =>
                        new
                        {
                            ItemId = i.ItemId,
                            ItemName = i.ItemName,
                            Value = i.Value,
                        }
                )
                .ToList<dynamic>();
        }

        private ProposalModel GetProposal(int id)
        {
            return _context.Proposal
                .Include(p => p.Person)
                .Include(pc => pc.ProposalContent)
                .ThenInclude(pc => pc.Item)
                .ThenInclude(i => i.ItemImageList)
                .FirstOrDefault(pp => pp.ProposalId == id);
        }

        private void PopulateViewBagUpdate(ProposalModel proposal)
        {
            ViewBag.ProposalPerson = proposal.Person;
            ViewBag.ProposalContent = proposal.ProposalContent;
            PopulateViewBagDefault();
        }

        private void PopulateViewBagDefault()
        {
            ViewBag.People = GetSelectListPeople();
            ViewBag.Items = GetSelectListItems();
            ViewBag.ItemsFull = GetItemList();
        }

        private async Task setProposalHistoryAsync(ProposalModel existentProposal)
        {
            ProposalHistoryModel ProposalHistory = new();
            ProposalHistory.ModificationDate = existentProposal.ModificationDate;
            ProposalHistory.Person = existentProposal.Person;
            ProposalHistory.Proposal = existentProposal;

            List<int> ItemsIdList = existentProposal.GetItemsIdList();
            ProposalHistory.ProposalContentArray = string.Join(",", ItemsIdList);

            await _context.ProposalHistory.AddAsync(ProposalHistory);
        }
    }
}
