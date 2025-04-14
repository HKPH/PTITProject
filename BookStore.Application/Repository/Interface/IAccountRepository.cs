using BookStore.Application.Repository.Interface;
using BookStore.Domain.Entities;

namespace BookStore.Application.Repository.Interface
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetByUsernameAsync(string username);
        Task<bool> CheckUsernameExistsAsync(string username);
        Task<PaginatedList<Account>> GetAllAsync(int page, int pageSize, string? searchUsername = null);


    }
}
