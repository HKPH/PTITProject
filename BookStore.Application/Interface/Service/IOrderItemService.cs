﻿using BookStore.Application.Dtos;


namespace BookStore.Application.Interface.Service
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
