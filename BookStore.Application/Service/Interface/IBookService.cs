using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Application.Service.Interface;

namespace BookStore.Application.Service.Interface
{
    public interface IBookService 
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<PaginatedList<BookDto>> GetAllAsync(int page, int pageSize, string? category = null, string? sortBy = null, bool isDescending = false, string? searchTerm = null);
        Task<BookDto> GetByIdAsync(int id);
        Task<BookDto> CreateAsync(BookDto bookDto);
        Task<bool> UpdateAsync(BookDto bookDto, int id);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<CategoryDto>> GetCategoryByBookId(int id);

        Task<bool> JoinCategoryToBook(int bookId, int categoryId);

    }
}
