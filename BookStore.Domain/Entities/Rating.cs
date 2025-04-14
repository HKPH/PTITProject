namespace BookStore.Domain.Entities;


public partial class Rating
{
    public int Id { get; set; }

    public int? BookId { get; set; }

    public int? UserId { get; set; }

    public int? Value { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Comment { get; set; }

    public string? Photo { get; set; }

    public bool? Active { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
