using BookStore.Infrastructure.Data;
using BookStore.Domain.Entities;
using BookStore.Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repository
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
