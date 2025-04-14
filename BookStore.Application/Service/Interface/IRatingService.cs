using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Application.Service.Interface;

namespace BookStore.Application.Service.Interface
{
    public interface IRatingService
    {
        Task<PaginatedList<RatingDto>> GetRatingByBookIdAsync(
            int bookId,
            int page,
            int pageSize,
            int? ratingValue = null,
            bool sortByDescending = false);
        Task<IEnumerable<RatingDto>> GetAllAsync();
        Task<RatingDto> GetByIdAsync(int id);
        Task<RatingDto> CreateAsync(RatingDto ratingDto);
        Task<bool> UpdateAsync(RatingDto ratingDto, int id);
        Task<bool> DeleteAsync(int id);
        Task<Dictionary<int, int>> GetRatingCountByBookIdAsync(int bookId);
    }

}
