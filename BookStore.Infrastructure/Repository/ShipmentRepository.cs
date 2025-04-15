using BookStore.Infrastructure.Data;
using BookStore.Application.Entities;
using Microsoft.EntityFrameworkCore;
using BookStore.Application.Interface.Repository;
namespace BookStore.Infrastructure.Repository
{
    public class ShipmentRepository : BaseRepository<Shipment>, IShipmentRepository
    {
        public ShipmentRepository(BookStoreContext context) : base(context)
        {
        }
    }

}
