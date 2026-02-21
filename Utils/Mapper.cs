using ShopASP.DTOs;
using ShopASP.DTOs.Orders;
using ShopASP.Models;

namespace ShopASP.Utils
{
    public class Mapper
    {
        public static Product ConsProductWithoutId(ProductDto dto)
        {
            return new Product
            {
                Description = dto.Description,
                Name = dto.Name,
                Price = dto.Price,
                IsActive = dto.IsActive
            };
        }

        public static Order ConsOrderWithoutId(OrderPersonalDetails dto, int userId)
        {
            return new Order
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                Street = dto.Street,
                Country = dto.Country,
                Recipient = dto.Recipient,
                City = dto.City,
                HouseNumber = dto.HouseNumber,
                ZipCode = dto.ZipCode
            };
        }

        public static OrderItem ConsOrderItem(OrderItemDto dto, int orderId)
        {
            return new OrderItem
            {
                OrderId = orderId,
                ProductId = dto.ProdId,
                PriceAtPurchase = dto.Price,
                Quantity = dto.Quantity
            };
        }

        public static void MapDtoOnProduct(Product prod, ProductDto dto)
        {
            prod.Id = dto.Id;
            prod.Price = dto.Price;
            prod.Description = dto.Description;
            prod.Name = dto.Name;
            prod.IsActive = dto.IsActive;
        }
    }
}
