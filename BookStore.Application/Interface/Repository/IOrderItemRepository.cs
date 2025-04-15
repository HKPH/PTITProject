using BookStore.Application.Entities;
namespace BookStore.Application.Interface.Repository
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
    }
}
