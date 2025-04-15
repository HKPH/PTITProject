using BookStore.Api.Attributes;
using BookStore.Application.Dtos;
using BookStore.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        // Số sách bán trong ngày
        [HttpGet("books-sold-by-day")]
        [Authorize(Policy = "AdminOnly")]
        [Cached(60)]
        public async Task<IActionResult> GetBooksSoldByDay(DateTime date)
        {
            var booksSold = await _statisticsService.GetBooksSoldByDayAsync(date);
            return Ok(booksSold);
        }


        // Sách bán theo năm
        [HttpGet("books-sold-by-year")]
        [Authorize(Policy = "AdminOnly")]
        [Cached(60)]
        public async Task<IActionResult> GetBooksSoldByYear(int year)
        {
            var booksSold = await _statisticsService.GetBooksSoldByYearAsync(year);
            return Ok(booksSold);
        }

        // Sách bán theo quý trong năm
        [HttpGet("books-sold-by-quarters")]
        [Authorize(Policy = "AdminOnly")]
        [Cached(60)]
        public async Task<IActionResult> GetBooksSoldByQuarters(int year)
        {
            var booksSold = await _statisticsService.GetBooksSoldByQuartersAsync(year);
            return Ok(booksSold);
        }

        // Doanh thu trong ngày
        [HttpGet("revenue-by-day")]
        [Authorize(Policy = "AdminOnly")]
        [Cached(60)]
        public async Task<IActionResult> GetRevenueByDay(DateTime date)
        {
            var revenue = await _statisticsService.GetRevenueByDayAsync(date);
            return Ok(revenue);
        }

        // Doanh thu theo năm
        [HttpGet("revenue-by-year")]
        [Authorize(Policy = "AdminOnly")]
        [Cached(60)]
        public async Task<IActionResult> GetRevenueByYear(int year)
        {
            var revenue = await _statisticsService.GetRevenueByYearAsync(year);
            return Ok(revenue);
        }

        // Doanh thu theo quý trong năm
        [HttpGet("revenue-by-quarters")]
        [Authorize(Policy = "AdminOnly")]
        [Cached(60)]
        public async Task<IActionResult> GetRevenueByQuarters(int year)
        {
            var revenue = await _statisticsService.GetRevenueByQuartersAsync(year);
            return Ok(revenue);
        }

        // Đếm số lượng đánh giá trong ngày
        [HttpGet("ratings-count-by-day")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<IActionResult> GetRatingsCountByDay(DateTime date)
        {
            var ratingsCount = await _statisticsService.GetRatingsCountByDayAsync(date);
            return Ok(ratingsCount);
        }

        // Đánh giá theo năm
        [HttpGet("ratings-by-year")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<IActionResult> GetRatingsByYear(int year)
        {
            var ratings = await _statisticsService.GetRatingsByYearAsync(year);
            return Ok(ratings);
        }

        // Đánh giá theo quý trong năm
        [HttpGet("ratings-by-quarters")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<IActionResult> GetRatingsByQuarters(int year)
        {
            var ratings = await _statisticsService.GetRatingsByQuartersAsync(year);
            return Ok(ratings);
        }
    }
}
