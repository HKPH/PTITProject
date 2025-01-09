using BookStore.Models;

namespace BookStore.Repository.Interface
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
