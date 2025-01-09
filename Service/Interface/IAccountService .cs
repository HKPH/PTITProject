using BookStore.Models;
using BookStore.Dtos;
using BookStore.Services.Interface;

namespace BookStore.Service.Interface
{
    public interface IAccountService
    {
        Task<PaginatedList<AccountDto>> GetAllAsync(int page, int pageSize, string? searchUsername = null);
        Task<AccountDto> GetByIdAsync(int id);
        Task<Account> CreateAsync(AccountDto accountDto);
        Task<bool> UpdateAsync(AccountDto accountDto, int Id);
        Task<bool> DeleteAsync(int id);
        Task<(AccountDto accountDto, UserDto userDto)> CreateAccountAndUserAsync(AccountDto accountDto, UserDto userDto);
        Task<(string token, int userId)> LoginAsync(string username, string password);
        Task<bool> ChangeActiveAsync(int id);
        Task<bool> ResetPasswordAsync(int id);
        Task<bool> ChangePasswordAsync(string oldPassword, int id, string newPassword);

    }
}
