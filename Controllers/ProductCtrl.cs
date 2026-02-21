using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using ShopASP.DTOs;
using ShopASP.Models;
using ShopASP.Utils;

namespace ShopASP.Controllers
{
    [Route("Product")]
    public class ProductCtrl : Base.ModController
    {
        private readonly ShopContext _ctx;

        private readonly IDataProtector _protector;

        public ProductCtrl(ShopContext ctx, IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector(Constants.CartCookiePurpose);
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            Product? product;

            if (User.IsInRole(Constants.AdminRoleName))
                product = ProductChecker.GetProduct(_ctx, id); // for admin to access inactive products.
            else product = ProductChecker.GetProductCheckValidity(_ctx, id);

            if (product == null) return RedirectError(Constants.RetCode.INVALID_PRODUCT_ID);

            return View(ProductDto.Cons(product));
        }

        [Route("Add")]
        [Authorize]
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            ShoppingCart cart = new(Request, _ctx, _protector);

            cart.Add(productId, quantity);

            cart.SaveCart(Response);

            TempData[Constants.TempDataInfo] = "Added to the cart.";

            return RedirectToAction(nameof(Index), new { id = productId });
        }
    }
}
