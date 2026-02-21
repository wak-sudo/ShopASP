namespace ShopASP.DTOs.Orders
{
    public class OrderViewDto
    {
        public int Id { get; init; }

        public required string Username { get; init; }

        public DateTime CreatedAt { get; init; }

    }
}