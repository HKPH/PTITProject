using BookStore.Application.Dtos;
using BookStore.Application.Entities;

namespace BookStore.Application.Interface.Service
{
    public interface IOrderService
    {
        Task<PaginatedList<OrderDto>> GetAllAsync(int page, int pageSize,string? searchTerm, string sortBy, bool sortDirection);
        Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId, int status);
        Task<OrderDto> GetByIdAsync(int id);
        Task<OrderDto> CreateAsync(OrderDto orderDto, ShipmentDto shipmentDto, PaymentDto paymentDto);
        Task<bool> UpdateAsync(OrderDto orderDto, int id);
        Task<bool> DeleteAsync(int id);
        Task<bool>  UpdateStatusAsync(int status, int id);
    }
}
