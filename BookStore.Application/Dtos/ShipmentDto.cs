namespace BookStore.Application.Dtos
{
    public class ShipmentDto
    {
        public int Id { get; set; }

        public int? ShippingAddressId { get; set; }

        public decimal? Fee { get; set; }

        public DateTime? DateReceived { get; set; }
    }

}
