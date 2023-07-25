using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using getQuote.Models;

namespace getQuote.Controllers
{
    public class ProposalController : Controller
    {
        private readonly ProposalBusiness _business;

        public ProposalController(ProposalBusiness business)
        {
            _business = business;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ProposalModel> proposals = await _business.GetProposals();
            return View(proposals);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateViewBagDefault();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProposalModel proposal)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBagDefault();
                return View(proposal);
            }

            await _business.AddAsync(proposal);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            ProposalModel? proposal = await _business.GetByIdAsync(id);
            if (proposal == null)
            {
                return NotFound();
            }
            ;

            await PopulateViewBagUpdate(proposal);
            return View(proposal);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProposalModel proposal)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBagUpdate(proposal);
                return View(proposal);
            }

            await _business.UpdateAsync(proposal);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _business.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Print(int id)
        {
            ProposalModel proposal = await _business.GetPrintByIdAsync(id);
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

        private async Task PopulateViewBagUpdate(ProposalModel proposal)
        {
            ViewBag.ProposalPerson = proposal.Person;
            ViewBag.ProposalContent = proposal.ProposalContent;
            await PopulateViewBagDefault();
        }

        private async Task PopulateViewBagDefault()
        {
            ViewBag.People = await _business.GetSelectListPeople();
            ViewBag.Items = await _business.GetSelectListItems();
            ViewBag.ItemsFull = await _business.GetDynamicItems();
        }
    }
}
