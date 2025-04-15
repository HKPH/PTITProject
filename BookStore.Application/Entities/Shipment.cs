namespace BookStore.Application.Entities;

public partial class Shipment
{
    public int Id { get; set; }

    public int? ShippingAddressId { get; set; }

    public decimal? Fee { get; set; }

    public DateTime? DateReceived { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ShippingAddress? ShippingAddress { get; set; }
}
