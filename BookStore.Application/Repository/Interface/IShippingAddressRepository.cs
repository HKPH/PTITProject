using BookStore.Application.Repository.Interface;
using BookStore.Domain.Entities;

namespace BookStore.Application.Repository.Interface
{
    public interface IShippingAddressRepository : IRepository<ShippingAddress>
    {
        Task<IEnumerable<ShippingAddress>> GetAllByUserIdAsync(int userId);
    }
}
