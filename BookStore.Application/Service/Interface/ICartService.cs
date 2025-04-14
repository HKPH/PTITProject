using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Application.Service.Interface;

namespace BookStore.Application.Service.Interface
{
    public interface ICartService
    {
        Task<IEnumerable<CartDto>> GetAllAsync();
        Task<CartDto> GetByIdAsync(int id);
        Task<CartDto> CreateAsync(CartDto cartDto);
        Task<bool> UpdateAsync(CartDto cartDto, int id);
        Task<bool> DeleteAsync(int id);

        Task<CartDto> GetCartByUserIdAsync(int userId);
    }
}
