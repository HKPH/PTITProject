using BookStore.Application.Entities;

namespace BookStore.Application.Interface.Repository
{
    public interface IShippingAddressRepository : IRepository<ShippingAddress>
    {
        Task<IEnumerable<ShippingAddress>> GetAllByUserIdAsync(int userId);
    }
}
