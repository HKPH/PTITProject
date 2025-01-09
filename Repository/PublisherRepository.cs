using BookStore.Data;
using BookStore.Models;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class PublisherRepository : BaseRepository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(BookStoreContext context) : base(context)
        {
        }
    }
}
