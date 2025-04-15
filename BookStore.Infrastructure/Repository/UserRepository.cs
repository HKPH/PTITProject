using BookStore.Infrastructure.Data;
using BookStore.Application.Entities;
using Microsoft.EntityFrameworkCore;
using BookStore.Application.Interface.Repository;
namespace BookStore.Infrastructure.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BookStoreContext context) : base(context)
        {
        }
    }
}
