using BookStore.Infrastructure.Data;
using BookStore.Application.Entities;
using Microsoft.EntityFrameworkCore;
using BookStore.Application.Interface.Repository;

namespace BookStore.Infrastructure.Repository
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        private readonly BookStoreContext _context;
        public OrderItemRepository(BookStoreContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }
    }
}
