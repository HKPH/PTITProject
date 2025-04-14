using BookStore.Application.Repository.Interface;
using BookStore.Domain.Entities;
namespace BookStore.Application.Repository.Interface
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<PaginatedList<Order>> GetAllAsync(
            int page,
            int pageSize,
            string? searchTerm,
            string sortBy,
            bool sortDirection);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId, int status);
    }
}
