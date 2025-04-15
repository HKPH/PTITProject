using BookStore.Application.Entities;
namespace BookStore.Application.Interface.Repository
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
    }
}
