using Microsoft.EntityFrameworkCore;
using ShopASP.DTOs.Orders;
using ShopASP.Models;

namespace ShopASP.Utils
{
    public class OrderDtoCons
    {
        public static OrderDto? Create(ShopContext ctx, int orderId)
        {
            var order = ctx.Orders.
                Where(el => el.Id == orderId).
                Include(obj => obj.User).
                FirstOrDefault();

            if (order == null) return null;

            return new()
            {
                General = ConsOrderViewDto(order),
                Details = ConsOrderPersonalDetails(order),
                Items = GetItems(ctx, orderId)
            };
        }

        private static OrderPersonalDetails ConsOrderPersonalDetails(Order order)
        {
            return new()
            {
                Recipient = order.Recipient,
                Country = order.Country,
                City = order.Country,
                ZipCode = order.ZipCode,
                Street = order.Street,
                HouseNumber = order.HouseNumber,
            };
        }

        private static OrderViewDto ConsOrderViewDto(Order order)
        {
            return new()
            {
                Username = order.User.Username,
                CreatedAt = order.CreatedAt,
                Id = order.Id,
            };
        }

        private static IReadOnlyCollection<OrderItemDto> GetItems(ShopContext ctx, int orderId)
        {
            return ctx.OrderItems.
                Where(el => el.OrderId == orderId).
                Include(obj => obj.Product).
                Select(el => new OrderItemDto
                {
                    ProdId = el.ProductId,
                    IsActive = el.Product.IsActive,
                    Name = el.Product.Name,
                    Price = el.Product.Price,
                    Quantity = el.Quantity
                }).
                ToList();
        }
    }
}
