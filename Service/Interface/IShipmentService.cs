using BookStore.Dtos;
using BookStore.Models;
using BookStore.Services.Interface;

namespace BookStore.Service.Interface
{
    public interface IShipmentService
    {
        Task<IEnumerable<ShipmentDto>> GetAllAsync();
        Task<ShipmentDto> GetByIdAsync(int id);
        Task<ShipmentDto> CreateAsync(ShipmentDto shipmentDto);
        Task<bool> UpdateAsync(ShipmentDto shipmentDto, int id);
        Task<bool> DeleteAsync(int id);
    }
}
