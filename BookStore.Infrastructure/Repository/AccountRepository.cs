using BookStore.Infrastructure.Data;
using BookStore.Domain.Entities;
using BookStore.Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
namespace BookStore.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BookStoreContext _context;

        public AccountRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }
        public async Task<PaginatedList<Account>> GetAllAsync(int page, int pageSize, string? searchUsername = null)
        {
            var query = _context.Accounts.AsQueryable();

            if (!string.IsNullOrEmpty(searchUsername) || !string.IsNullOrWhiteSpace(searchUsername))
            {
                query = query.Where(b => b.Username.Contains(searchUsername));
            }

            var totalCount = await query.CountAsync();

            var accounts = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Account>(accounts, totalCount, page, pageSize);
        }
        public async Task<Account> GetByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<Account> CreateAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account> GetByUsernameAsync(string username)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            return await _context.Accounts.AnyAsync(a => a.Username == username);
        }

        public async Task<bool> UpdateAsync(Account entity, int id)
        {
            var existingAccount = await GetByIdAsync(id);

            if (existingAccount == null)
            {
                return false;
            }

            _context.Entry(existingAccount).CurrentValues.SetValues(entity);
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var account = await GetByIdAsync(id);
            if (account == null)
            {
                return false;
            }
            _context.Accounts.Remove(account);
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
    }
}
