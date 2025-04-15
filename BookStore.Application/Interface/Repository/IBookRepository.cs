using BookStore.Application.Entities;

namespace BookStore.Application.Interface.Repository
{
    public interface IBookRepository: IRepository<Book>
    {
        Task<PaginatedList<Book>> GetAllAsync(int page, int pageSize, string? category = null, string? sortBy = null, bool sortDirection = false, string? searchTerm = null);
        Task<IEnumerable<Category>> GetCategoryByBookId(int id);
        Task<bool> JoinCategoryToBook(int bookId, int categoryId);
        Task<bool> DeactivateBookAsync(int bookId);

    }
}
