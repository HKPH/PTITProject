namespace BookStore.Dtos
{
    public class CreateOrderDto
    {
        public OrderDto Order { get; set; }
        public ShipmentDto Shipment { get; set; }
        public PaymentDto Payment { get; set; }
    }
}
