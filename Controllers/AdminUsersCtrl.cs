using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopASP.DTOs;
using ShopASP.Models;
using ShopASP.Utils;

namespace ShopASP.Controllers
{
    [Route("Admin/Users")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class AdminUsersCtrl : Controller
    {
        private readonly ShopContext _ctx;

        public AdminUsersCtrl(ShopContext ctx) => _ctx = ctx;

        [HttpGet]
        public IActionResult Index(int? pageNumber)
        {
            IQueryable<UserDto> query = _ctx.Users.
                 Select(el => new UserDto
                 {
                     UserName = el.Username,
                     IsAdmin = el.IsAdmin,
                 });

            return View(new Paging<UserDto>(query, pageNumber));
        }
    }
}
