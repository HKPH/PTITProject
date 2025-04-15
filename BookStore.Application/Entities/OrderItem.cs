﻿namespace BookStore.Application.Entities;

public partial class OrderItem
{
    public int OrderId { get; set; }

    public int BookId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public int Id { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
