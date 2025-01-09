using BookStore.Models;

namespace BookStore.Repository.Interface
{
    public interface IShippingAddressRepository : IRepository<ShippingAddress>
    {
        Task<IEnumerable<ShippingAddress>> GetAllByUserIdAsync(int userId);
    }
}
