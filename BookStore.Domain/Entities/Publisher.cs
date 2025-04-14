namespace BookStore.Domain.Entities;


public partial class Publisher
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
