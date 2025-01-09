using BookStore.Dtos;
using BookStore.Models;
using BookStore.Services.Interface;

namespace BookStore.Service.Interface
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDto>> GetAllAsync();
        Task<OrderItemDto> GetByIdAsync(int id);
        Task<OrderItemDto> CreateAsync(OrderItemDto orderItemDto);
        Task<bool> UpdateAsync(OrderItemDto orderItemDto, int id);
        Task<bool> DeleteAsync(int id);
    }
}
