using Microsoft.AspNetCore.Mvc;
using getQuote.Models;

namespace getQuote.Controllers
{
    public class ProposalHistoryController : BaseController
    {
        private readonly ProposalHistoryBusiness _business;

        public ProposalHistoryController(ProposalHistoryBusiness business)
        {
            _business = business;
        }

        public async Task<IActionResult> Print(int id)
        {
            ProposalModel proposal = await _business.GetProposalFromHistory(id);
            return View(proposal);
        }
    }
}
