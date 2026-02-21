using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopASP.DTOs;
using ShopASP.Models;
using ShopASP.Utils;

namespace ShopASP.Controllers
{
    [Route("Admin/EditProduct")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class AdminProductEditCtrl : Base.ModController
    {
        private readonly ShopContext _ctx;

        public AdminProductEditCtrl(ShopContext ctx) => _ctx = ctx;

        [HttpGet]
        public IActionResult Index(int prodcutId)
        {
            var prod = ProductChecker.GetProduct(_ctx, prodcutId);

            if (prod == null) return RedirectError(Constants.RetCode.INVALID_PRODUCT_ID);

            return View(ProductDto.Cons(prod));
        }

        [Route("Edit")]
        [HttpPost]
        public IActionResult Edit(ProductDto dto)
        {
            var prod = _ctx.Products.FirstOrDefault(item => item.Id == dto.Id);

            if (prod == null) return RedirectError(Constants.RetCode.INVALID_PRODUCT_ID);

            Mapper.MapDtoOnProduct(prod, dto);

            _ctx.SaveChanges();

            TempData[Constants.TempDataInfo] = "Product sucessfuly edited.";

            return RedirectToAction(nameof(Index), new { prodcutId = prod.Id });
        }
    }
}
