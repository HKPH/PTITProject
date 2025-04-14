using System;
using System.Collections.Generic;

namespace BookStore.Application.Dtos;

public partial class OrderItemDto
{
    public int OrderId { get; set; }

    public int BookId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public int Id { get; set; }
}

