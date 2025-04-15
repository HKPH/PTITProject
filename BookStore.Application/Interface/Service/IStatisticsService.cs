using BookStore.Application.Dtos;
namespace BookStore.Application.Interface.Service
{
    public interface IStatisticsService
    {
        Task<int> GetBooksSoldByDayAsync(DateTime date);
        Task<List<BookSoldDTO>> GetBooksSoldForLast7DaysAsync();
        Task<List<MonthlyBookSoldDTO>> GetBooksSoldByYearAsync(int year);
        Task<List<QuarterlyBookSoldDTO>> GetBooksSoldByQuartersAsync(int year);

        Task<decimal> GetRevenueByDayAsync(DateTime date);
        Task<List<RevenueDTO>> GetRevenueForLast7DaysAsync();
        Task<List<MonthlyRevenueDTO>> GetRevenueByYearAsync(int year);
        Task<List<QuarterlyRevenueDTO>> GetRevenueByQuartersAsync(int year);

        Task<List<RatingByStarDTO>> GetRatingsByStarAsync(DateTime startDate, DateTime endDate);
        Task<List<RatingByStarDTO>> GetRatingsCountByDayAsync(DateTime date);
        Task<List<RatingDTO>> GetRatingsForLast7DaysAsync();
        Task<List<MonthlyRatingDTO>> GetRatingsByYearAsync(int year);
        Task<List<QuarterlyRatingDTO>> GetRatingsByQuartersAsync(int year);
    }
}
