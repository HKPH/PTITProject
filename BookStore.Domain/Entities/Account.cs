namespace BookStore.Domain.Entities;

public partial class Account
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? Role { get; set; }

    public string? Email { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? Active { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
