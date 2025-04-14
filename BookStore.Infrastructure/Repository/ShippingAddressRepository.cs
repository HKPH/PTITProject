using BookStore.Infrastructure.Data;
using BookStore.Domain.Entities;
using BookStore.Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repository
{
    public class ShippingAddressRepository : BaseRepository<ShippingAddress>, IShippingAddressRepository
    {
        private readonly BookStoreContext _context;
        public ShippingAddressRepository(BookStoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShippingAddress>> GetAllByUserIdAsync(int userId)
        {
            return await _context.ShippingAddresses
                     .Where(sa => sa.UserId == userId)
                     .ToListAsync();
        }
    }
}
