using BookStore.Application.Repository.Interface;
using BookStore.Domain.Entities;
namespace BookStore.Application.Repository.Interface
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
    }
}
