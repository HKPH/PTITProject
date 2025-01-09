using BookStore.Dtos;
using BookStore.Models;
using BookStore.Services.Interface;

namespace BookStore.Service.Interface
{
    public interface IShippingAddressService
    {
        Task<ShippingAddressDto> GetById(int id);
        Task<IEnumerable<ShippingAddressDto>> GetAllByUserIdAsync(int userId);
        Task<ShippingAddressDto> CreateAsync(ShippingAddressDto ShippingAddressDto);
        Task<bool> UpdateAsync(ShippingAddressDto ShippingAddressDto, int id);
        Task<bool> DeleteAsync(int id);
    }
}
