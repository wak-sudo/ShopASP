using Microsoft.AspNetCore.Mvc;
using ShopASP.Utils;

namespace ShopASP.Controllers.Base
{
    public class ModController : Controller
    {
        public RedirectToActionResult RedirectError(Constants.RetCode err) => base.RedirectToAction(nameof(ErrorCtrl.Index), nameof(ErrorCtrl),
            new { errorCode = (int)err });
    }
}
