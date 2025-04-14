namespace BookStore.Application.Dtos;

public partial class OrderDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? Status { get; set; }

    public int ShipmentId { get; set; }

    public int PaymentId { get; set; }

    public decimal? TotalPrice { get; set; }

    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

}
