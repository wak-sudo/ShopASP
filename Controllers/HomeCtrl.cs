using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ShopASP.Models;
using ShopASP.Utils;

namespace ShopASP.Controllers
{
    [Route("")]
    public class HomeCtrl : Controller
    {
        private readonly ShopContext _ctx;

        public HomeCtrl(ShopContext ctx) => _ctx = ctx;

        [HttpGet]
        public IActionResult Index(string? searchQuery, int? page)
        {
            const int minSearchLenght = 4;

            if (searchQuery != null && searchQuery.Length <= minSearchLenght)
            {
                ViewBag.Error = $"Search query lenght must be at least {minSearchLenght}.";
                return View();
            }

            IQueryable<Product> targetQuery = _ctx.Products.Where(el => el.IsActive);
            if (!String.IsNullOrEmpty(searchQuery))
                targetQuery = targetQuery.Where(el => el.Name.Contains(searchQuery) || el.Description.Contains(searchQuery));

            return View(new Paging<Product>(targetQuery, page));
        }

        [Route("Logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
