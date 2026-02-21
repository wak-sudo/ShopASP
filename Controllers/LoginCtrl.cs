using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShopASP.DTOs;
using ShopASP.Models;
using ShopASP.Utils;
using System.Security.Claims;

namespace ShopASP.Controllers
{
    [Route("Login")]
    public class LoginCtrl : Controller
    {
        private readonly ShopContext _ctx;

        public LoginCtrl(ShopContext ctx) => _ctx = ctx;

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(HomeCtrl.Index), nameof(HomeCtrl));

            LoginDTO model = new();

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(LoginDTO model)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(HomeCtrl.Index), nameof(HomeCtrl));

            if (model.UserName == null)
            {
                ViewBag.Error = "The user name field is empty.";
                return View(model);
            }

            if (model.Password == null)
            {
                ViewBag.Error = "The password field is empty.";
                return View(model);
            }

            var loggedUser = Helpers.CheckGetUser(_ctx, model.UserName, model.Password);

            if (loggedUser == null)
            {
                ViewBag.Error = "Wrong credentials.";
                return View(model);
            }

            string userRole = (loggedUser.IsAdmin) ? Constants.AdminRoleName : "";

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, loggedUser.Id.ToString()),
                new(ClaimTypes.Name, loggedUser.Username),
                new(ClaimTypes.Role, userRole),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction(nameof(HomeCtrl.Index), nameof(HomeCtrl));
        }
    }
}
