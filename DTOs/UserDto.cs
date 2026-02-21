namespace ShopASP.DTOs
{
    public class UserDto
    {
        public required string UserName { get; init; }

        public bool IsAdmin { get; init; }
    }
}
