using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Application.Dtos;
using BookStore.Application.Service.Interface;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/Cart
        [HttpGet]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetAllCarts()
        {
            var carts = await _cartService.GetAllAsync();
            return Ok(carts);
        }

        // GET: api/Cart/5
        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetCartById(int id)
        {
            var cart = await _cartService.GetByIdAsync(id);
            if (cart == null)
            {
                return NotFound(new { message = "Cart not found." });
            }
            return Ok(cart);
        }

        // POST: api/Cart
        [HttpPost]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> CreateCart([FromBody] CartDto cartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCart = await _cartService.CreateAsync(cartDto);
            return CreatedAtAction(nameof(GetCartById), new { id = createdCart.Id }, createdCart);
        }

        // PUT: api/Cart/5
        [HttpPut("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> UpdateCart(int id, [FromBody] CartDto cartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _cartService.UpdateAsync(cartDto, id);
            if (!updated)
            {
                return NotFound(new { message = "Cart not found." });
            }

            return Ok(new { message = "Cart updated successfully." });
        }

        // DELETE: api/Cart/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> DeleteCart(int id)
        {
            var deleted = await _cartService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Cart not found." });
            }

            return Ok(new { message = "Cart deleted successfully." });
        }

        // GET: api/Cart/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            var cartDto = await _cartService.GetCartByUserIdAsync(userId);

            if (cartDto == null)
            {
                return NotFound(new { message = "Cart not found for this user." });
            }

            return Ok(cartDto);
        }
    }
}
