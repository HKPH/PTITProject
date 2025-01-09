﻿using BookStore.Models;

namespace BookStore.Repository.Interface
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetByUsernameAsync(string username);
        Task<bool> CheckUsernameExistsAsync(string username);
        Task<PaginatedList<Account>> GetAllAsync(int page, int pageSize, string? searchUsername = null);


    }
}
