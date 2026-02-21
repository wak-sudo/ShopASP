using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopASP.Models;
using ShopASP.Utils;

namespace ShopASP.Controllers
{
    [Route("Admin/OrderDetails")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class AdminOrderDetailsCtrl : Controller
    {
        private readonly ShopContext _ctx;

        public AdminOrderDetailsCtrl(ShopContext ctx) => _ctx = ctx;

        [HttpGet]
        public IActionResult Index(int orderId)
        {
            var res = OrderDtoCons.Create(_ctx, orderId);
            if (res == null) return NotFound();
            return View(res);
        }
    }
}
