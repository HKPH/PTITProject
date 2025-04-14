﻿using BookStore.Application.Dtos;


namespace BookStore.Application.Service.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CategoryDto categoryDto);
        Task<bool> UpdateAsync(CategoryDto categoryDto, int id);
        Task<bool> DeleteAsync(int id);
    }
}
