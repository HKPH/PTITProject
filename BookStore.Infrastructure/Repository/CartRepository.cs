using BookStore.Infrastructure.Data;
using BookStore.Application.Entities;
using Microsoft.EntityFrameworkCore;
using BookStore.Application.Interface.Repository;

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
