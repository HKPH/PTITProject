namespace BookStore.Dtos
{
    public class ShippingAddressDto
    {
        public int Id { get; set; }

        public string? Address { get; set; }

        public string? Note { get; set; }

        public string? CustomerNumber { get; set; }

        public int? UserId { get; set; }
    }

}
