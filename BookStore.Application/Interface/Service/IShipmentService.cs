using BookStore.Application.Dtos;


namespace BookStore.Application.Interface.Service
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
