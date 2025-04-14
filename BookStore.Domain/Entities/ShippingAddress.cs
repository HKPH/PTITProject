namespace BookStore.Domain.Entities;


public partial class ShippingAddress
{
    public int Id { get; set; }

    public string? Address { get; set; }

    public string? Note { get; set; }

    public string? CustomerNumber { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

    public virtual User? User { get; set; }
}
