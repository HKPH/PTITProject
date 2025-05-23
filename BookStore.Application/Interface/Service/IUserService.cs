﻿using BookStore.Application.Dtos;


namespace BookStore.Application.Interface.Service
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(UserDto userDto);
        Task<bool> UpdateAsync(UserDto userDto, int id);
        Task<bool> DeleteAsync(int id);
    }
}
