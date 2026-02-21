using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopASP.DTOs;
using ShopASP.Models;
using ShopASP.Utils;

namespace ShopASP.Controllers
{
    [Route("Admin/AddProduct")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class AdminProductAddCtrl : Controller
    {
        private readonly ShopContext _ctx;

        public AdminProductAddCtrl(ShopContext ctx) => _ctx = ctx;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductDto dto)
        {
            var product = Mapper.ConsProductWithoutId(dto);

            _ctx.Products.Add(product);

            _ctx.SaveChanges();

            TempData[Constants.TempDataInfo] = "Product added.";

            return RedirectToAction(nameof(Index));
        }
    }
}
