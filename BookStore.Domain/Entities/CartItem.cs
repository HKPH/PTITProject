namespace BookStore.Domain.Entities;

public partial class CartItem
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int CartId { get; set; }

    public int? Quantity { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Cart Cart { get; set; } = null!;
}
