using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Application.Entities;
using BookStore.Application.Interface.Repository;
using BookStore.Application.Interface.Service;

namespace BookStore.Application.Service
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IMapper _mapper;

        public ShipmentService(IShipmentRepository shipmentRepository, IMapper mapper)
        {
            _shipmentRepository = shipmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShipmentDto>> GetAllAsync()
        {
            var shipments = await _shipmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ShipmentDto>>(shipments);
        }

        public async Task<ShipmentDto> GetByIdAsync(int id)
        {
            var shipment = await _shipmentRepository.GetByIdAsync(id);
            if (shipment == null)
            {
                throw new Exception("Shipment not found.");
            }
            return _mapper.Map<ShipmentDto>(shipment);
        }

        public async Task<ShipmentDto> CreateAsync(ShipmentDto shipmentDto)
        {
            var shipment = _mapper.Map<Shipment>(shipmentDto);
            var createdShipment = await _shipmentRepository.CreateAsync(shipment);
            if (createdShipment == null)
            {
                throw new Exception("Failed to create Shipment.");
            }
            return _mapper.Map<ShipmentDto>(createdShipment);
        }

        public async Task<bool> UpdateAsync(ShipmentDto shipmentDto, int id)
        {
            var checkShipment = await _shipmentRepository.GetByIdAsync(id);
            if (checkShipment == null)
            {
                throw new Exception("Shipment not found.");
            }

            var shipment = _mapper.Map<Shipment>(shipmentDto);
            return await _shipmentRepository.UpdateAsync(shipment, id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var checkShipment = await _shipmentRepository.GetByIdAsync(id);
            if (checkShipment == null)
            {
                throw new Exception("Shipment not found.");
            }

            return await _shipmentRepository.DeleteAsync(id);
        }
    }
}