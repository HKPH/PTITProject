using BookStore.Data;
using BookStore.Models;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(BookStoreContext context) : base(context)
        {
        }
    }

}
