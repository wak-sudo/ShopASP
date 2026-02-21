using ShopASP.Models;

namespace ShopASP.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        public bool IsActive { get; init; }

        public required string Name { get; init; }

        public decimal Price { get; init; }

        public required string Description { get; init; }

        public static ProductDto Cons(Product obj)
        {
            return new ProductDto
            {
                Id = obj.Id,
                IsActive = obj.IsActive,
                Name = obj.Name,
                Price = obj.Price,
                Description = obj.Description
            };
        }
    }
}
