using BookStore.Models;

namespace BookStore.Repository.Interface
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);
        Task<bool> CartItemExistsAsync(int cartId, int bookId);
    }
}
