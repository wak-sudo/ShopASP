using Microsoft.AspNetCore.Mvc;
using ShopASP.Utils;

namespace ShopASP.Controllers
{
    [Route("Error")]
    public class ErrorCtrl : Controller
    {

        [HttpGet]
        public IActionResult Index(int errorCode)
        {
            const string unkErrorMsg = "Unknown error code.";

            try
            {
                Constants.RetCode code = (Constants.RetCode)errorCode;
                Constants.Map.TryGetValue(code, out string? message);
                if (message == null) ViewBag.Error = unkErrorMsg;
                else ViewBag.Error = message;
            }
            catch
            {
                ViewBag.Error = unkErrorMsg;
            }

            return View();
        }
    }
}
