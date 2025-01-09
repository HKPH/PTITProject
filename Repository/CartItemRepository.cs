using BookStore.Data;
using BookStore.Models;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class CartItemRepository : BaseRepository<CartItem>, ICartItemRepository
    {
        private readonly BookStoreContext _context;
        public CartItemRepository(BookStoreContext context) : base(context) 
        { 
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }
        public async Task<bool> CartItemExistsAsync(int cartId, int bookId)
        {
            return await _context.CartItems
                .AnyAsync(ci => ci.CartId == cartId && ci.BookId == bookId);
        }
    }
}
