using BookStore.Dtos;
using BookStore.Models;
using BookStore.Services.Interface;

namespace BookStore.Service.Interface
{
    public interface ICartItemService 
    {
        Task<IEnumerable<CartItemDto>> GetAllAsync();
        Task<CartItemDto> GetByIdAsync(int id);
        Task<CartItemDto> CreateAsync(CartItemDto cartItemDto);
        Task<bool> UpdateAsync(CartItemDto cartItemDto, int id);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<CartItemDto>> GetCartItemsByCartIdAsync(int cartId);
    }
}
