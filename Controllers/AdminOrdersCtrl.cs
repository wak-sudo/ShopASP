using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopASP.DTOs.Orders;
using ShopASP.Models;
using ShopASP.Utils;

namespace ShopASP.Controllers
{
    [Route("Admin/Orders")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class AdminOrdersCtrl : Controller
    {
        private readonly ShopContext _ctx;

        public AdminOrdersCtrl(ShopContext ctx) => _ctx = ctx;

        public IActionResult Index(int? pageNumber)
        {
            IQueryable<OrderViewDto> query = _ctx.Orders.
                Select(el => new OrderViewDto()
                {
                    Username = el.User.Username,
                    CreatedAt = el.CreatedAt,
                    Id = el.Id,
                });

            var model = new Paging<OrderViewDto>(query, pageNumber);

            return View(model);
        }
    }
}
