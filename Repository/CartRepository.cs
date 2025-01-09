using BookStore.Data;
using BookStore.Models;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
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
