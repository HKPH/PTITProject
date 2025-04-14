using BookStore.Infrastructure.Data;
using BookStore.Domain.Entities;
using BookStore.Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repository
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        private readonly BookStoreContext _context;
        public CartRepository(BookStoreContext context) : base(context)
        {
            _context = context;

        }
        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
