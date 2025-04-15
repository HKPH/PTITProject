using BookStore.Application.Entities;

namespace BookStore.Application.Interface.Repository
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);
        Task<bool> CartItemExistsAsync(int cartId, int bookId);
    }
}
