using BookStore.Data;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using BookStore.Dtos;

namespace BookStore.Repository
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly BookStoreContext _context;

        public StatisticsRepository(BookStoreContext context)
        {
            _context = context;
        }

        // Số lượng sách bán theo ngày
        public async Task<int> GetBooksSoldByDayAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            return await _context.OrderItems
                .Where(oi => oi.Order.OrderDate >= startDate &&
                             oi.Order.OrderDate < endDate &&
                             (oi.Order.Status == 3 || oi.Order.Status == 4))
                .SumAsync(oi => oi.Quantity ?? 0);
        }

        // Số lượng sách bán trong 7 ngày gần nhất
        public async Task<List<BookSoldDTO>> GetBooksSoldForLast7DaysAsync()
        {
            var result = new List<BookSoldDTO>();

            for (int i = 0; i < 7; i++)
            {
                var date = DateTime.UtcNow.Date.AddDays(-i);
                var booksSold = await GetBooksSoldByDayAsync(date);
                result.Add(new BookSoldDTO { Date = date, BooksSold = booksSold });
            }

            return result.OrderByDescending(r => r.Date).ToList();
        }

        // Số lượng sách bán trong năm theo tháng
        public async Task<List<MonthlyBookSoldDTO>> GetBooksSoldByYearAsync(int year)
        {
            var result = new List<MonthlyBookSoldDTO>();

            for (int month = 1; month <= 12; month++)
            {
                var booksSold = await _context.OrderItems
                    .Where(oi => oi.Order.OrderDate.HasValue &&
                                 oi.Order.OrderDate.Value.Year == year &&
                                 oi.Order.OrderDate.Value.Month == month &&
                                 (oi.Order.Status == 3 || oi.Order.Status == 4))
                    .SumAsync(oi => oi.Quantity ?? 0);

                result.Add(new MonthlyBookSoldDTO { Month = month, BooksSold = booksSold });
            }

            return result;
        }

        // Số lượng sách bán trong năm theo quý
        public async Task<List<QuarterlyBookSoldDTO>> GetBooksSoldByQuartersAsync(int year)
        {
            var result = new List<QuarterlyBookSoldDTO>();

            for (int quarter = 1; quarter <= 4; quarter++)
            {
                var startMonth = (quarter - 1) * 3 + 1;
                var endMonth = startMonth + 2;

                var booksSold = await _context.OrderItems
                    .Where(oi => oi.Order.OrderDate.HasValue &&
                                 oi.Order.OrderDate.Value.Year == year &&
                                 oi.Order.OrderDate.Value.Month >= startMonth &&
                                 oi.Order.OrderDate.Value.Month <= endMonth &&
                                 (oi.Order.Status == 3 || oi.Order.Status == 4))
                    .SumAsync(oi => oi.Quantity ?? 0);

                result.Add(new QuarterlyBookSoldDTO { Quarter = quarter, BooksSold = booksSold });
            }

            return result;
        }

        // Doanh thu theo ngày
        public async Task<decimal> GetRevenueByDayAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            return await _context.Orders
                .Where(o => o.OrderDate >= startDate &&
                            o.OrderDate < endDate &&
                            (o.Status == 3 || o.Status == 4))
                .SumAsync(o => o.TotalPrice ?? 0);
        }

        // Doanh thu trong 7 ngày gần nhất
        public async Task<List<RevenueDTO>> GetRevenueForLast7DaysAsync()
        {
            var result = new List<RevenueDTO>();

            for (int i = 0; i < 7; i++)
            {
                var date = DateTime.UtcNow.Date.AddDays(-i);
                var revenue = await GetRevenueByDayAsync(date);
                result.Add(new RevenueDTO { Date = date, Revenue = revenue });
            }

            return result.OrderByDescending(r => r.Date).ToList();
        }

        // Doanh thu trong năm theo tháng
        public async Task<List<MonthlyRevenueDTO>> GetRevenueByYearAsync(int year)
        {
            var result = new List<MonthlyRevenueDTO>();

            for (int month = 1; month <= 12; month++)
            {
                var revenue = await _context.Orders
                    .Where(o => o.OrderDate.HasValue &&
                                o.OrderDate.Value.Year == year &&
                                o.OrderDate.Value.Month == month &&
                                (o.Status == 3 || o.Status == 4))
                    .SumAsync(o => o.TotalPrice ?? 0);

                result.Add(new MonthlyRevenueDTO { Month = month, Revenue = revenue });
            }

            return result;
        }

        // Doanh thu trong năm theo quý
        public async Task<List<QuarterlyRevenueDTO>> GetRevenueByQuartersAsync(int year)
        {
            var result = new List<QuarterlyRevenueDTO>();

            for (int quarter = 1; quarter <= 4; quarter++)
            {
                var startMonth = (quarter - 1) * 3 + 1;
                var endMonth = startMonth + 2;

                var revenue = await _context.Orders
                    .Where(o => o.OrderDate.HasValue &&
                                o.OrderDate.Value.Year == year &&
                                o.OrderDate.Value.Month >= startMonth &&
                                o.OrderDate.Value.Month <= endMonth &&
                                (o.Status == 3 || o.Status == 4))
                    .SumAsync(o => o.TotalPrice ?? 0);

                result.Add(new QuarterlyRevenueDTO { Quarter = quarter, Revenue = revenue });
            }

            return result;
        }

        public async Task<List<RatingByStarDTO>> GetRatingsByStarAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Ratings
                .Where(r => r.CreateDate >= startDate && r.CreateDate < endDate && r.Active == true)
                .GroupBy(r => r.Value)
                .Select(g => new RatingByStarDTO
                {
                    Star = g.Key,
                    Count = g.Count()
                })
                .OrderBy(r => r.Star)
                .ToListAsync();
        }

        public async Task<List<RatingByStarDTO>> GetRatingsCountByDayAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);
            var ratings = await GetRatingsByStarAsync(startDate, endDate);
            return ratings;
        }

        public async Task<List<RatingDTO>> GetRatingsForLast7DaysAsync()
        {
            var result = new List<RatingDTO>();

            for (int i = 0; i < 7; i++)
            {
                var date = DateTime.UtcNow.Date.AddDays(-i);
                var startDate = date.Date;
                var endDate = startDate.AddDays(1);
                var ratings = await GetRatingsByStarAsync(startDate, endDate);
                result.Add(new RatingDTO { Date = date, RatingsCountByStar = ratings });
            }

            return result.OrderByDescending(r => r.Date).ToList();
        }

        public async Task<List<MonthlyRatingDTO>> GetRatingsByYearAsync(int year)
        {
            var result = new List<MonthlyRatingDTO>();

            for (int month = 1; month <= 12; month++)
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1);
                var ratingsByStar = await GetRatingsByStarAsync(startDate, endDate);

                result.Add(new MonthlyRatingDTO
                {
                    Month = month,
                    RatingsCountByStar = ratingsByStar
                });
            }

            return result;
        }

        public async Task<List<QuarterlyRatingDTO>> GetRatingsByQuartersAsync(int year)
        {
            var result = new List<QuarterlyRatingDTO>();

            for (int quarter = 1; quarter <= 4; quarter++)
            {
                var startMonth = (quarter - 1) * 3 + 1;
                var endMonth = startMonth + 2;

                var startDate = new DateTime(year, startMonth, 1);
                var endDate = new DateTime(year, endMonth, DateTime.DaysInMonth(year, endMonth));
                var ratingsByStar = await GetRatingsByStarAsync(startDate, endDate);

                result.Add(new QuarterlyRatingDTO
                {
                    Quarter = quarter,
                    RatingsCountByStar = ratingsByStar
                });
            }

            return result;
        }


    }
}