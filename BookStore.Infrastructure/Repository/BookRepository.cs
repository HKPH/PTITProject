using AutoMapper;
using BookStore.Infrastructure.Data;
using BookStore.Application.Dtos;
using BookStore.Application.Entities;
using Microsoft.EntityFrameworkCore;
using BookStore.Application.Interface.Repository;

namespace BookStore.Infrastructure.Repository
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly BookStoreContext _context;

        public BookRepository(BookStoreContext context) : base(context)
        {
            _context = context;
        }
        public async Task<PaginatedList<Book>> GetAllAsync(
            int page,
            int pageSize,
            string? category = null,
            string? sortBy = "id",
            bool sortDirection = true, 
            string? searchTerm = null)
        {
            var query = _context.Books.AsQueryable();
            query = query.Where(b => b.Active == true);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(b => b.Categories.Any(c => c.Name == category));
            }

            if (!string.IsNullOrEmpty(searchTerm) || !string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(b => b.Title.Contains(searchTerm));
            }

            query = sortBy?.ToLower() switch
            {
                "title" => sortDirection ? query.OrderBy(b => b.Title) : query.OrderByDescending(b => b.Title),
                "price" => sortDirection ? query.OrderBy(b => b.Price) : query.OrderByDescending(b => b.Price),
                "id" => sortDirection ? query.OrderBy(b => b.Id) : query.OrderByDescending(b => b.Id),
                _ => query.OrderByDescending(b => b.Id) 
            };

            var totalCount = await query.CountAsync();

            var books = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Book>(books, totalCount, page, pageSize);
        }

        public async Task<bool> DeactivateBookAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return false;
            }
            book.Active = false;
            var changes = await _context.SaveChangesAsync();
            return changes > 0;

        }


        public async Task<IEnumerable<Category>> GetCategoryByBookId(int bookId)
        {
            var book = await _context.Books
                                     .Include(b => b.Categories)
                                     .FirstOrDefaultAsync(b => b.Id == bookId);

            return book?.Categories ?? Enumerable.Empty<Category>();
        }
        public async Task<bool> JoinCategoryToBook(int bookId, int categoryId)
        {

            var book = await _context.Books.Include(b => b.Categories)
                                            .FirstOrDefaultAsync(b => b.Id == bookId);
            var category = await _context.Categories.FindAsync(categoryId);

            if (book == null || category == null)
            {
                return false;
            }
            if (book.Categories.Any(c => c.Id == categoryId))
            {
                return true;
            }
            book.Categories.Add(category);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
