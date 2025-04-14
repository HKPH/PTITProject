namespace BookStore.Domain.Entities;


public partial class Book
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Author { get; set; }

    public int? PublisherId { get; set; }

    public decimal? Price { get; set; }

    public DateTime? PublicationDate { get; set; }

    public bool? Active { get; set; }

    public int? StockQuantity { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Publisher? Publisher { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
