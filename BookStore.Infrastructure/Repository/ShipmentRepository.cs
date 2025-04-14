using BookStore.Infrastructure.Data;
using BookStore.Domain.Entities;
using BookStore.Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;
namespace BookStore.Infrastructure.Repository
{
    public class ShipmentRepository : BaseRepository<Shipment>, IShipmentRepository
    {
        public ShipmentRepository(BookStoreContext context) : base(context)
        {
        }
    }

}
