using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Application.Entities;
using BookStore.Application.Interface.Repository;
using BookStore.Application.Interface.Service;

namespace BookStore.Application.Service
{
    public class ShippingAddressService : IShippingAddressService
    {
        private readonly IShippingAddressRepository _shippingAddressRepository;
        private readonly IMapper _mapper;

        public ShippingAddressService(IShippingAddressRepository repository, IMapper mapper)
        {
            _shippingAddressRepository = repository;
            _mapper = mapper;
        }

        public async Task<ShippingAddressDto> CreateAsync(ShippingAddressDto shippingAddressDto)
        {
            var shippingAddress = _mapper.Map<ShippingAddress>(shippingAddressDto);
            var createdShippingAddress = await _shippingAddressRepository.CreateAsync(shippingAddress);

            if (createdShippingAddress == null)
            {
                throw new Exception("Failed to create shipping address.");
            }

            return _mapper.Map<ShippingAddressDto>(createdShippingAddress);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var checkShippingAddress = await _shippingAddressRepository.GetByIdAsync(id);
            if (checkShippingAddress == null)
            {
                throw new Exception("Shipping address not found.");
            }

            return await _shippingAddressRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ShippingAddressDto>> GetAllByUserIdAsync(int userId)
        {
            var shippingAddresses = await _shippingAddressRepository.GetAllByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ShippingAddressDto>>(shippingAddresses);
        }

        public async Task<ShippingAddressDto> GetById(int id)
        {
            var shippingAddress = await _shippingAddressRepository.GetByIdAsync(id);
            if (shippingAddress == null)
            {
                throw new Exception("Shipping address not found.");
            }
            return _mapper.Map<ShippingAddressDto>(shippingAddress);
        }

        public async Task<bool> UpdateAsync(ShippingAddressDto shippingAddressDto, int id)
        {
            var checkShippingAddress = await _shippingAddressRepository.GetByIdAsync(id);
            if (checkShippingAddress == null)
            {
                throw new Exception("Shipping address not found.");
            }

            var shippingAddress = _mapper.Map<ShippingAddress>(shippingAddressDto);
            return await _shippingAddressRepository.UpdateAsync(shippingAddress, id);
        }
    }
}
 