using Microsoft.AspNetCore.DataProtection;
using ShopASP.DTOs.Orders;
using ShopASP.Models;

namespace ShopASP.Utils
{
    public class ShoppingCart
    {
        private readonly Dictionary<int, int> _cart = new();

        private readonly ShopContext _ctx;

        private readonly IDataProtector _protector;

        public ShoppingCart(HttpRequest req, ShopContext ctx, IDataProtector protector)
        {
            _ctx = ctx;
            _protector = protector;

            string? cookie = req.Cookies[Constants.CartCookieName];

            if (cookie != null)
            {
                try
                {
                    _cart = Helpers.Deserialize<Dictionary<int, int>>(_protector, cookie) ?? new Dictionary<int, int>();
                }
                catch
                {
                    _cart = new();
                }
            }
            else _cart = new();
        }

        public bool IsEmpty() => _cart.Count == 0;

        public void Add(int productId, int quantity) => _cart[productId] = quantity;

        public bool IsCorrect()
        {
            var pairsList = GetPairsInCart();

            foreach (var pair in pairsList)
            {
                var productID = pair.Item1;
                var quantity = pair.Item2;

                if (quantity < 1) return false;

                if (ProductChecker.GetProductCheckValidity(_ctx, productID) == null)
                    return false;
            }

            return true;
        }

        public IReadOnlyCollection<(int, int)> GetPairsInCart() => _cart.Select(el => (el.Key, el.Value)).ToList();

        public IReadOnlyCollection<OrderItemDto> GetContent()
        {
            var pairsList = GetPairsInCart();
            var res = new List<OrderItemDto>();

            foreach (var pair in pairsList)
            {
                var productID = pair.Item1;
                var quantity = pair.Item2;

                var prod = ProductChecker.GetProduct(_ctx, productID);

                if (prod == null) continue;

                res.Add(new OrderItemDto
                {
                    Name = prod.Name,
                    IsActive = prod.IsActive,
                    Price = prod.Price,
                    ProdId = prod.Id,
                    Quantity = quantity
                });
            }

            return res;
        }

        public string Serialize() => Helpers.Serialize(_protector, _cart);

        public bool Remove(int productId)
        {
            if (!_cart.ContainsKey(productId)) return false;
            _cart.Remove(productId);
            return true;
        }

        public void RemoveAll() => _cart.Clear();

        public void SaveCart(HttpResponse resp)
        {
            CookieOptions options = new()
            {
                Expires = DateTime.Now.AddDays(7),
                SameSite = SameSiteMode.Strict    // Ochrona przed CSRF (?)
            };

            resp.Cookies.Append(Constants.CartCookieName, Serialize(), options);
        }
    }
}
