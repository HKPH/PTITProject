using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Application.Dtos;
using BookStore.Application.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using BookStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BookStore.Api.Attributes;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;
        private readonly BookStoreContext _context;
        public PublisherController(IPublisherService publisherService, BookStoreContext context)
        {
            _publisherService = publisherService;
            _context = context;
        }
        [HttpGet("check")]
        public async Task<ActionResult<int?>> CheckCategoryExists(string name)
        {
            var category = await _context.Publishers
                .FirstOrDefaultAsync(c => c.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));

            if (category == null)
            {
                return NotFound("Category not found");
            }

            return Ok(category.Id);
        }
        // GET: api/Publisher
        [HttpGet]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetAllPublishers()
        {
            var publishers = await _publisherService.GetAllAsync();
            return Ok(publishers);
        }

        // GET: api/Publisher/5
        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        [Cached(60)]
        public async Task<IActionResult> GetPublisherById(int id)
        {
            var publisher = await _publisherService.GetByIdAsync(id);
            if (publisher == null)
            {
                return NotFound(new { message = "Publisher not found." });
            }
            return Ok(publisher);
        }

        // POST: api/Publisher
        [HttpPost]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> CreatePublisher([FromBody] PublisherDto publisherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPublisher = await _publisherService.CreateAsync(publisherDto);
            return CreatedAtAction(nameof(GetPublisherById), new { id = createdPublisher.Id }, createdPublisher);
        }

        // PUT: api/Publisher/5
        [HttpPut("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> UpdatePublisher(int id, [FromBody] PublisherDto publisherDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _publisherService.UpdateAsync(publisherDto, id);
            if (!updated)
            {
                return NotFound(new { message = "Publisher not found." });
            }

            return Ok(new { message = "Publisher updated successfully." });
        }

        // DELETE: api/Publisher/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> DeletePublisher(int id)
        {
            var deleted = await _publisherService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Publisher not found." });
            }

            return Ok(new { message = "Publisher deleted successfully." });
        }
    }
}
