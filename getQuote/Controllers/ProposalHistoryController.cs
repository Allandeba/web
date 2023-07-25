using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using getQuote.Models;
using getQuote.DAO;

namespace getQuote.Controllers
{
    public class ProposalHistoryController : Controller
    {
        private readonly ILogger<ProposalHistoryController> _logger;
        private readonly ApplicationDBContext _context;

        public ProposalHistoryController(
            ILogger<ProposalHistoryController> logger,
            ApplicationDBContext context
        )
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Print(int id)
        {
            var proposalHistory = await _context.ProposalHistory
                .Include(p => p.Person)
                .Include(p => p.Proposal)
                .FirstOrDefaultAsync(ph => ph.ProposalHistoryId == id);

            ProposalModel proposal = new ProposalModel
            {
                Person = proposalHistory.Person,
                ProposalContent = new List<ProposalContentModel>(),
            };

            var items = await _context.Item
                .Include(i => i.ItemImageList)
                .Where(i => proposalHistory.ProposalContentJSON.GetItemIds().Contains(i.ItemId))
                .ToListAsync();

            proposalHistory.ProposalContentJSON.ProposalContentItems.OrderBy(p => p.ItemId);
            items.OrderBy(i => i.ItemId);
            for (int i = 0; i < items.Count; i++)
            {
                ProposalContentModel proposalContent = new ProposalContentModel
                {
                    Proposal = proposal,
                    Item = items[i],
                    Quantity = Int32.Parse(
                        proposalHistory.ProposalContentJSON.ProposalContentItems[i].Quantity
                    ),
                };

                proposal.ProposalContent.Add(proposalContent);
            }

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
    }
}
