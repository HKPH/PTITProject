using BookStore.Infrastructure.Data;
using BookStore.Domain.Entities;
using BookStore.Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly BookStoreContext _context;
        public OrderRepository(BookStoreContext context) : base(context)
        {
            _context = context;
        }
        public async Task<PaginatedList<Order>> GetAllAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string sortBy = "OrderDate",
            bool sortDirection = false)
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(o => _context.Users
                    .Any(u => u.Id == o.UserId &&
                              (u.Name.Contains(searchTerm) || u.Phone.Contains(searchTerm))));
            }

            query = sortBy.ToLower() switch
            {
                "id" => sortDirection
                    ? query.OrderBy(o => o.Id)
                    : query.OrderByDescending(o => o.Id),
                "userid" => sortDirection
                    ? query.OrderBy(o => o.UserId)
                    : query.OrderByDescending(o => o.UserId),
                "orderdate" => sortDirection
                    ? query.OrderBy(o => o.OrderDate)
                    : query.OrderByDescending(o => o.OrderDate),
                "status" => sortDirection
                    ? query.OrderBy(o => o.Status)
                    : query.OrderByDescending(o => o.Status),
                _ => query.OrderBy(o => o.OrderDate)
            };

            var totalCount = await query.CountAsync();

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Order>(orders, totalCount, page, pageSize);
        }


        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId, int status)
        {
            var query = _context.Orders
                .Where(o => o.UserId == userId);

            if (status != 0) 
            {
                query = query.Where(o => o.Status == status);
            }

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders;
        }


    }
}
