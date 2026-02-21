namespace ShopASP.DTOs.Orders
{
    public class OrderPersonalDetails
    {
        public required string Recipient { get; init; }

        public required string Country { get; init; }

        public required string City { get; init; }

        public required string ZipCode { get; init; }

        public required string Street { get; init; }

        public required string HouseNumber { get; init; }
    }
}