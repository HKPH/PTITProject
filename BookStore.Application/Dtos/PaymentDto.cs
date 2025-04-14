﻿namespace BookStore.Application.Dtos
{
    public class PaymentDto
    {
        public int Id { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? PaymentMethod { get; set; }

        public decimal? Amount { get; set; }
    }


}
