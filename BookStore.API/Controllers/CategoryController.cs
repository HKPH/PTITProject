using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Application.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BookStore.Infrastructure.Data;
using BookStore.Api.Attributes;
using BookStore.Application.Dtos;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly BookStoreContext _context;

        public CategoryController(ICategoryService categoryService, BookStoreContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }
        [HttpGet("check")]
        public async Task<ActionResult<int?>> CheckCategoryExists(string name)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));

            if (category == null)
            {
                return NotFound("Category not found");
            }

            return Ok(category.Id);
        }
        // GET: api/Category
        [HttpGet]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetAllCategorys()
        {
            var categorys = await _categoryService.GetAllAsync();
            return Ok(categorys);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        [Cached(60)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found." });
            }
            return Ok(category);
        }

        // POST: api/Category
        [HttpPost]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCategory = await _categoryService.CreateAsync(categoryDto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _categoryService.UpdateAsync(categoryDto, id);
            if (!updated)
            {
                return NotFound(new { message = "Category not found." });
            }

            return Ok(new { message = "Category updated successfully." });
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Category not found." });
            }

            return Ok(new { message = "Category deleted successfully." });
        }
    }
}
