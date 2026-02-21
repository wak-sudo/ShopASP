using ShopASP.Models;

namespace ShopASP.Utils
{
    public class ProductChecker
    {
        public static Product? GetProductCheckValidity(ShopContext ctx, int? prodId)
        {
            if (prodId == null || prodId < 0) return null;

            var selProduct = ctx.Products.FirstOrDefault(item => item.Id == prodId);

            if (selProduct == null || !selProduct.IsActive) return null;

            return selProduct;
        }

        public static Product? GetProduct(ShopContext ctx, int? prodId)
        {
            if (prodId == null) return null;

            if (prodId < 0) return null;

            var selProduct = ctx.Products.FirstOrDefault(item => item.Id == prodId);

            return selProduct;
        }
    }
}
