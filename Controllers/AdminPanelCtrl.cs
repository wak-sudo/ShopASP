using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopASP.Utils;

namespace ShopASP.Controllers
{
    [Route("Admin/Panel")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class AdminPanelCtrl : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
