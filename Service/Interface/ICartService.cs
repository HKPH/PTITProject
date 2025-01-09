using BookStore.Dtos;
using BookStore.Models;
using BookStore.Services.Interface;

namespace BookStore.Service.Interface
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
