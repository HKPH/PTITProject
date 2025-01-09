using BookStore.Repository.Interface;
using BookStore.Service.Interface;
using BookStore.Dtos;
using BookStore.Repository;

namespace BookStore.Service
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _repository;

        public StatisticsService(IStatisticsRepository repository)
        {
            _repository = repository;
        }
 
        public Task<int> GetBooksSoldByDayAsync(DateTime date) => _repository.GetBooksSoldByDayAsync(date);

        public Task<List<BookSoldDTO>> GetBooksSoldForLast7DaysAsync() => _repository.GetBooksSoldForLast7DaysAsync();

        public Task<List<MonthlyBookSoldDTO>> GetBooksSoldByYearAsync(int year) => _repository.GetBooksSoldByYearAsync(year);

        public Task<List<QuarterlyBookSoldDTO>> GetBooksSoldByQuartersAsync(int year) => _repository.GetBooksSoldByQuartersAsync(year);

        public Task<decimal> GetRevenueByDayAsync(DateTime date) => _repository.GetRevenueByDayAsync(date);

        public Task<List<RevenueDTO>> GetRevenueForLast7DaysAsync() => _repository.GetRevenueForLast7DaysAsync();

        public Task<List<MonthlyRevenueDTO>> GetRevenueByYearAsync(int year) => _repository.GetRevenueByYearAsync(year);

        public Task<List<QuarterlyRevenueDTO>> GetRevenueByQuartersAsync(int year) => _repository.GetRevenueByQuartersAsync(year);

        public async Task<List<RatingByStarDTO>> GetRatingsByStarAsync(DateTime startDate, DateTime endDate)
        {
            return await _repository.GetRatingsByStarAsync(startDate, endDate);
        }

        public async Task<List<RatingByStarDTO>> GetRatingsCountByDayAsync(DateTime date)
        {
            return await _repository.GetRatingsCountByDayAsync(date);
        }

        public async Task<List<RatingDTO>> GetRatingsForLast7DaysAsync()
        {
            return await _repository.GetRatingsForLast7DaysAsync();
        }

        public async Task<List<MonthlyRatingDTO>> GetRatingsByYearAsync(int year)
        {
            return await _repository.GetRatingsByYearAsync(year);
        }

        public async Task<List<QuarterlyRatingDTO>> GetRatingsByQuartersAsync(int year)
        {
            return await _repository.GetRatingsByQuartersAsync(year);
        }
    }
}
