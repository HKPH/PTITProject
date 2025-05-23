﻿using BookStore.Application.Dtos;

namespace BookStore.Application.Interface.Service

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
