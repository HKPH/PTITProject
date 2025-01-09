using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Dtos;
using BookStore.Service.Interface;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        // GET: api/CartItem
        [HttpGet]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetAllCartItems()
        {
            var cartItems = await _cartItemService.GetAllAsync();
            return Ok(cartItems);
        }

        // GET: api/CartItem/5
        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetCartItemById(int id)
        {
            var cartItem = await _cartItemService.GetByIdAsync(id);
            if (cartItem == null)
            {
                return NotFound(new { message = "CartItem not found." });
            }
            return Ok(cartItem);
        }

        // POST: api/CartItem
        [HttpPost]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> CreateCartItem([FromBody] CartItemDto cartItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCartItem = await _cartItemService.CreateAsync(cartItemDto);
            return CreatedAtAction(nameof(GetCartItemById), new { id = createdCartItem.Id }, createdCartItem);
        }

        // PUT: api/CartItem/5
        [HttpPut("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CartItemDto cartItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _cartItemService.UpdateAsync(cartItemDto, id);
            if (!updated)
            {
                return NotFound(new { message = "CartItem not found." });
            }

            return Ok(new { message = "CartItem updated successfully." });
        }

        // DELETE: api/CartItem/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var deleted = await _cartItemService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "CartItem not found." });
            }

            return Ok(new { message = "CartItem deleted successfully." });
        }

        // GET: api/CartItem/cart/{cartId}
        [HttpGet("cart/{cartId}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetCartItemsByCartId(int cartId)
        {
            try
            {
                var cartItems = await _cartItemService.GetCartItemsByCartIdAsync(cartId);

                if (cartItems == null || !cartItems.Any())
                {
                    return NotFound(new { Message = "Không tìm thấy CartItem nào cho CartId này." });
                }

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý yêu cầu.", Error = ex.Message });
            }
        }
    }
}
