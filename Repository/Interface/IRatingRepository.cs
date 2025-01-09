using BookStore.Models;

namespace BookStore.Repository.Interface
{
    public interface IRatingRepository : IRepository<Rating>
    {
        Task<PaginatedList<Rating>> GetRatingsByBookIdAsync(
                int bookId,
                int page,
                int pageSize,
                int? ratingValue = null, // (1-5)
                bool sortByDescending = false);
        Task<Dictionary<int, int>> GetRatingCountByBookIdAsync(int bookId);
    }
}
