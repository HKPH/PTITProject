using BookStore.Infrastructure.Data;
using BookStore.Application.Entities;
using Microsoft.EntityFrameworkCore;
using BookStore.Application.Interface.Repository;

namespace BookStore.Infrastructure.Repository
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(BookStoreContext context) : base(context)
        {
        }
    }

}
