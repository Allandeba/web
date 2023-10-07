using System.Net;
using System.Security.Claims;
using getQuote.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index));
            }
            try
            {
                await _business.Login(login);
            }
            catch (Exception exception)
            {
                await _business.SaveLoginLog(GetLoginLog(login, LoginLogStatus.Failed));
                throw exception;
            }

            await _business.SaveLoginLog(GetLoginLog(login, LoginLogStatus.Success));

            await StartAuthentication(login);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }

        private async Task StartAuthentication(LoginModel login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, "User"),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            AuthenticationProperties authenticationProperties = new AuthenticationProperties()
            {
                IsPersistent = login.Remember
            };

            await HttpContext.SignInAsync(claimsPrincipal, authenticationProperties);
        }

        private LoginLogModel GetLoginLog(LoginModel login, LoginLogStatus loginLogStatus)
        {
            LoginLogModel loginLog = new();
            loginLog.Username = login.Username;
            loginLog.Password = login.Password;
            loginLog.Hostname = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).HostName;
            loginLog.RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            loginLog.DateTime = DateTime.Now;
            loginLog.Status = loginLogStatus;

            return loginLog;
        }
    }
}
