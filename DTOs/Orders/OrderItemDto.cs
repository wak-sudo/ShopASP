namespace ShopASP.DTOs.Orders
{
    public class OrderItemDto
    {
        public int ProdId { get; set; }

        public bool IsActive { get; set; }

        public required string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}