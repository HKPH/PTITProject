using System;
using System.Collections.Generic;

namespace BookStore.Dtos;

public partial class CartItemDto
{
    public int CartId { get; set; }

    public int BookId { get; set; }

    public int? Quantity { get; set; }

    public int Id { get; set; }

}
