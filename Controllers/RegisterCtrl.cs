using Microsoft.AspNetCore.Mvc;
using ShopASP.DTOs;
using ShopASP.Models;
using ShopASP.Utils;

namespace ShopASP.Controllers
{
    [Route("Register")]
    public class RegisterCtrl : Controller
    {
        private readonly ShopContext _ctx;

        public RegisterCtrl(ShopContext ctx) => _ctx = ctx;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(RegisterDto dto)
        {
            if (dto.UserName == null)
            {
                ViewBag.Error = "The user name field is empty.";
                return View(dto);
            }

            if (dto.Password == null)
            {
                ViewBag.Error = "The password field is empty.";
                return View(dto);
            }

            var user = _ctx.Users.FirstOrDefault(item => item.Username == dto.UserName);

            if (user != null)
            {
                ViewBag.Error = "The user name is already taken.";
                View(dto);
            }

            string salt = Helpers.GenSalt();
            string password = Helpers.PasswordToHash(dto.Password, salt);

            _ctx.Users.Add(new User
            {
                Username = dto.UserName,
                PasswordHash = password,
                Salt = salt,
                IsAdmin = false,
            });

            _ctx.SaveChanges();

            return RedirectToAction(nameof(Sucess));
        }

        [Route("Sucess")]
        [HttpGet]
        public IActionResult Sucess()
        {
            return View();
        }
    }
}
