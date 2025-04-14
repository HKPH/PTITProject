using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Application.Dtos;
using BookStore.Application.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using BookStore.Api.Attributes;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        // GET: api/Rating
        [HttpGet]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetAllRatings()
        {
            var ratings = await _ratingService.GetAllAsync();
            return Ok(ratings);
        }

        // GET: api/Rating/ 
        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        [Cached(60)]
        public async Task<IActionResult> GetRatingById(int id)
        {
            var rating = await _ratingService.GetByIdAsync(id);
            if (rating == null)
            {
                return NotFound(new { message = "Rating not found." });
            }
            return Ok(rating);
        }

        [HttpGet("book/{bookId}")]
        [Authorize(Policy = "UserOrAdmin")]
        [Cached(60)]
        public async Task<IActionResult> GetRatingsByBookId(
                    int bookId,
                    [FromQuery] int page = 1,
                    [FromQuery] int pageSize = 10,
                    [FromQuery] int? ratingValue = null,
                    [FromQuery] bool sortByDescending = false)
        {
            try
            {
                var ratings = await _ratingService.GetRatingByBookIdAsync(
                    bookId, page, pageSize, ratingValue, sortByDescending);

                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while fetching ratings: {ex.Message}");
            }
        }

        // POST: api/Rating
        [HttpPost]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> CreateRating([FromBody] RatingDto ratingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRating = await _ratingService.CreateAsync(ratingDto);
            return CreatedAtAction(nameof(GetRatingById), new { id = createdRating.Id }, createdRating);
        }

        // PUT: api/Rating/5
        [HttpPut("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> UpdateRating(int id, [FromBody] RatingDto ratingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _ratingService.UpdateAsync(ratingDto, id);
            if (!updated)
            {
                return NotFound(new { message = "Rating not found." });
            }

            return Ok(new { message = "Rating updated successfully." });
        }

        // DELETE: api/Rating/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> DeleteRating(int id)
        {
            var deleted = await _ratingService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Rating not found." });
            }

            return Ok(new { message = "Rating deleted successfully." });
        }

        // GET: api/Rating/5
        [HttpGet("ratings/{bookId}")]
        [Cached(60)]
        public async Task<IActionResult> GetRatingCountByBookId(int bookId)
        {
            var ratingCounts = await _ratingService.GetRatingCountByBookIdAsync(bookId);

            if (ratingCounts == null)
            {
                return NotFound(new { Message = "No ratings found for this book" });
            }

            return Ok(ratingCounts);
        }
    }
}
