using getQuote.Models;
using Microsoft.AspNetCore.Mvc;

namespace getQuote.Controllers
{
    public class LoginController : BaseController
    {
        private readonly LoginBusiness _business;

        public LoginController(LoginBusiness business)
        {
            _business = business;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel Login)
        {
            if (!ModelState.IsValid)
            {
                return View(Index);
            }

            await _business.Login(Login);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
