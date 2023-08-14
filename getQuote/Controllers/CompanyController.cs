using getQuote;
using getQuote.Controllers;
using getQuote.Models;
using Microsoft.AspNetCore.Mvc;

public class CompanyController : BaseController
{
    private readonly CompanyBusiness _business;

    public CompanyController(CompanyBusiness companyBusiness)
    {
        _business = companyBusiness;
    }

    public async Task<IActionResult> Manipulate()
    {
        CompanyModel? existentCompany = await _business.GetAllAsync();
        if (existentCompany != null)
        {
            return RedirectToAction(nameof(Update), new { id = existentCompany.CompanyId });
        }

        return RedirectToAction(nameof(Create));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Create(CompanyModel company)
    {
        if (company == null)
        {
            return BadRequest(ModelState);
        }

        if (company.FormImageFile != null)
        {
            company.SetNewImage();
        }

        if (!ModelState.IsValid)
        {
            return View(company);
        }

        await _business.AddAsync(company);
        return Redirect("/");
    }

    public async Task<IActionResult> Update(int id)
    {
        CompanyModel company = await _business.GetByIdAsync(id);
        return View(company);
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Update(CompanyModel company)
    {
        if (company == null)
        {
            return BadRequest(ModelState);
        }

        if (company.FormImageFile != null)
        {
            company.SetNewImage();
        }

        if (!ModelState.IsValid)
        {
            return View(company);
        }

        await _business.UpdateAsync(company);
        return Redirect("/");
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _business.RemoveAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
