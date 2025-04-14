using BookStore.Application.Repository.Interface;
using BookStore.Domain.Entities;

namespace BookStore.Application.Repository.Interface
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);
        Task<bool> CartItemExistsAsync(int cartId, int bookId);
    }
}
