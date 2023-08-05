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

        public IActionResult ExportToPDF(int id)
        {
            string url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            url = $"{url}/{ControllerContext.RouteData.Values["controller"]}/{nameof(Print)}/{id}";

            ExportToPDFModel exportPDF = new();
            return File(
                exportPDF.GetPDF(url),
                System.Net.Mime.MediaTypeNames.Application.Pdf,
                $"ProposalHistory-{id}.pdf"
            );
        }
    }
}
