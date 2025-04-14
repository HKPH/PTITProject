using BookStore.Application.Repository.Interface;
using BookStore.Domain.Entities;

namespace BookStore.Application.Repository.Interface
{
    public interface IBookRepository: IRepository<Book>
    {
        Task<PaginatedList<Book>> GetAllAsync(int page, int pageSize, string? category = null, string? sortBy = null, bool sortDirection = false, string? searchTerm = null);
        Task<IEnumerable<Category>> GetCategoryByBookId(int id);
        Task<bool> JoinCategoryToBook(int bookId, int categoryId);
        Task<bool> DeactivateBookAsync(int bookId);

    }
}
