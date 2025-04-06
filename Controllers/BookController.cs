using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookStore.Dtos;
using BookStore.Service.Interface;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using BookStore.Attributes;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Book
        [HttpGet]
        [Authorize(Policy = "UserOrAdmin")]
        [Cached(60)]
        public async Task<IActionResult> GetAllBooks(int page = 1, int pageSize = 10, string ? category = null, string ? sortBy = null, bool isDescending = false, string ? searchTerm = null)
        {
            var paginatedBooks = await _bookService.GetAllAsync(page, pageSize, category, sortBy, isDescending, searchTerm);
            return Ok(paginatedBooks);
        }


        // GET: api/Book/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        [Cached(60)]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound("Book not found");
            }
            return Ok(book);
        }

        // POST: api/Book
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateBook([FromBody] BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBook = await _bookService.CreateAsync(bookDto);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        // PUT: api/Book/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _bookService.UpdateAsync(bookDto, id);
            if (!updated)
            {
                return NotFound("Book not found");
            }

            return NoContent();
        }

        // DELETE: api/Book/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var deleted = await _bookService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound("Book not found");
            }

            return NoContent();
        }

        // GET: api/Book/{id}/categories
        [HttpGet("{id}/categories")]
        [Authorize(Policy = "UserOrAdmin")]
        [Cached(60)]
        public async Task<IActionResult> GetCategoryByBookId(int id)
        {
            var categories = await _bookService.GetCategoryByBookId(id);

            if (categories == null)
            {
                return NotFound("Category not found");
            }

            return Ok(categories);
        }

        // POST: api/Book/{bookId}/categories/{categoryId}
        [HttpPost("{bookId}/categories/{categoryId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddCategoryJoinBook(int bookId, int categoryId)
        {
            var success = await _bookService.JoinCategoryToBook(bookId, categoryId);

            if (!success)
            {
                return BadRequest("Cant join category to book");
            }

            return Ok("Join successfuly");
        }
    }
}
