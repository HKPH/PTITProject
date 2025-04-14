using BookStore.Infrastructure.Data;
using BookStore.Domain.Entities;
using BookStore.Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repository
{
    public class CategoryRepository : BaseRepository<Category>,ICategoryRepository
    {
        public CategoryRepository(BookStoreContext context) : base(context)
        {
        }
    }
}
