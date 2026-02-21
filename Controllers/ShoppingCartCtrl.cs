using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using ShopASP.DTOs.Orders;
using ShopASP.Models;
using ShopASP.Utils;
using System.Security.Claims;

namespace ShopASP.Controllers
{
    [Route("Cart")]
    [Authorize]
    public class ShoppingCartCtrl : Base.ModController
    {
        private readonly ShopContext _ctx;

        private readonly IDataProtector _protector;

        public ShoppingCartCtrl(ShopContext ctx, IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector(Constants.CartCookiePurpose);
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ShoppingCart cart = new(Request, _ctx, _protector);

            return View(cart.GetContent());
        }

        [Route("Remove")]
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            ShoppingCart cart = new(Request, _ctx, _protector);

            cart.Remove(productId);

            cart.SaveCart(Response);

            return RedirectToAction(nameof(Index));
        }

        [Route("MakeOrder")]
        [HttpPost]
        public IActionResult MakeOrder(OrderPersonalDetails dto)
        {
            ShoppingCart cart = new(Request, _ctx, _protector);

            if (cart.IsEmpty())
            {
                TempData[Constants.TempDataInfo] = "The cart is empty.";
                return RedirectToAction(nameof(Index));
            }

            if (!cart.IsCorrect())
            {
                TempData[Constants.TempDataInfo] = "Edit the cart. Some items are unavailable or invalid.";
                return RedirectToAction(nameof(Index));
            }

            Order ord;
            try
            {
                ord = Mapper.ConsOrderWithoutId(dto, Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            }

            catch
            {
                return RedirectError(Constants.RetCode.INVALID_ORDER);
            }

            if (!TryAddOrderWithItems(ord, cart.GetContent()))
            {
                TempData[Constants.TempDataInfo] = "Provided data is wrong.";
                return RedirectToAction(nameof(Index));
            }

            cart.RemoveAll();
            cart.SaveCart(Response);

            TempData["CanAccessOrderTaken"] = true;
            return RedirectToAction(nameof(OrderTaken));
        }

        [Route("Taken")]
        public IActionResult OrderTaken()
        {
            if (TempData["CanAccessOrderTaken"] == null)
                return RedirectToAction(nameof(HomeCtrl.Index), nameof(HomeCtrl));
            return View();
        }

        private bool TryAddOrderWithItems(Order ord, IReadOnlyCollection<OrderItemDto> items)
        {
            using var transaction = _ctx.Database.BeginTransaction();
            try
            {
                _ctx.Orders.Add(ord);

                _ctx.SaveChanges(); // after this the order should recieve an id.

                foreach (var item in items)
                    _ctx.OrderItems.Add(Mapper.ConsOrderItem(item, ord.Id));

                _ctx.SaveChanges();

                transaction.Commit();

                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }
    }
}
