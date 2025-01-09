using BookStore.Data;
using BookStore.Models;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class CategoryRepository : BaseRepository<Category>,ICategoryRepository
    {
        public CategoryRepository(BookStoreContext context) : base(context)
        {
        }
    }
}
