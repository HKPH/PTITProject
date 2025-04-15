using BookStore.Application.Entities;
namespace BookStore.Application.Interface.Repository
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
