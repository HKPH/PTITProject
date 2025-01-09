using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class Payment
{
    public int Id { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal? Amount { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
