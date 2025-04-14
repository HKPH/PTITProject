using BookStore.Infrastructure.Data;
using BookStore.Domain.Entities;
using BookStore.Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repository
{
    public class RatingRepository : BaseRepository<Rating>, IRatingRepository
    {
        private readonly BookStoreContext _context;
        public RatingRepository(BookStoreContext context) : base(context)
        {
            _context = context;
        }
        public async Task<PaginatedList<Rating>> GetRatingsByBookIdAsync(
                int bookId,
                int page,
                int pageSize,
                int? ratingValue = null,
                bool sortByDescending = false)
        {
            var query = _context.Ratings.AsQueryable();

            query = query.Where(r => r.BookId == bookId);

            if (ratingValue.HasValue)
            {
                query = query.Where(r => r.Value == ratingValue);
            }

            query = sortByDescending
                ? query.OrderByDescending(r => r.CreateDate)
                : query.OrderBy(r => r.CreateDate);

            var totalCount = await query.CountAsync();
            var ratings = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Rating>(ratings, totalCount, page, pageSize);
        }

        public async Task<Dictionary<int, int>> GetRatingCountByBookIdAsync(int bookId)
        {
            var ratingCounts = await _context.Ratings
                .Where(r => r.BookId == bookId)
                .GroupBy(r => r.Value)
                .Select(g => new { RatingValue = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = new Dictionary<int, int>
            {
                { 1, ratingCounts.Where(r => r.RatingValue == 1).Select(r => r.Count).FirstOrDefault() },
                { 2, ratingCounts.Where(r => r.RatingValue == 2).Select(r => r.Count).FirstOrDefault() },
                { 3, ratingCounts.Where(r => r.RatingValue == 3).Select(r => r.Count).FirstOrDefault() },
                { 4, ratingCounts.Where(r => r.RatingValue == 4).Select(r => r.Count).FirstOrDefault() },
                { 5, ratingCounts.Where(r => r.RatingValue == 5).Select(r => r.Count).FirstOrDefault() }
            };

            return result;
        }
    }
}
