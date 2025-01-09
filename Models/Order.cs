﻿using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? Status { get; set; }

    public int ShipmentId { get; set; }

    public int PaymentId { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Payment? Payment { get; set; }

    public virtual Shipment? Shipment { get; set; }
}
