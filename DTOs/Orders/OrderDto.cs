namespace ShopASP.DTOs.Orders
{
    public class OrderDto
    {
        public required OrderViewDto General { get; init; }

        public required OrderPersonalDetails Details { get; init; }

        public required IReadOnlyCollection<OrderItemDto> Items { get; init; }


    }
}