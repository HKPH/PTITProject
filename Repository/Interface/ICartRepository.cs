using BookStore.Models;

namespace BookStore.Repository.Interface
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
    }
}
