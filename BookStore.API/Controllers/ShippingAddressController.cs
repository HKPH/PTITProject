using Microsoft.AspNetCore.Mvc;
using BookStore.Application.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using BookStore.Application.Dtos;

namespace BookStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingAddressController : ControllerBase
    {
        private readonly IShippingAddressService _shippingAddressService;

        public ShippingAddressController(IShippingAddressService shippingAddressService)
        {
            _shippingAddressService = shippingAddressService;
        }

        [HttpPost]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> CreateShippingAddress([FromBody] ShippingAddressDto shippingAddressDto)
        {
            if (shippingAddressDto == null)
            {
                return BadRequest("Shipping address data is required.");
            }

            try
            {
                var createdAddress = await _shippingAddressService.CreateAsync(shippingAddressDto);
                return CreatedAtAction(nameof(GetShippingAddressById), new { id = createdAddress.Id }, createdAddress);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetShippingAddressesByUserId(int userId)
        {
            try
            {
                var addresses = await _shippingAddressService.GetAllByUserIdAsync(userId);
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> UpdateShippingAddress(int id, [FromBody] ShippingAddressDto shippingAddressDto)
        {
            if (shippingAddressDto == null)
            {
                return BadRequest("Shipping address data is required.");
            }

            try
            {
                var updated = await _shippingAddressService.UpdateAsync(shippingAddressDto, id);
                if (updated)
                {
                    return NoContent();
                }

                return NotFound("Shipping address not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> DeleteShippingAddress(int id)
        {
            try
            {
                var deleted = await _shippingAddressService.DeleteAsync(id);
                if (deleted)
                {
                    return NoContent();
                }

                return NotFound("Shipping address not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetShippingAddressById(int id)
        {
            try
            {
                var addresses = await _shippingAddressService.GetById(id);
                if (addresses == null)
                {
                    return NotFound("Shipping address not found.");
                }

                return Ok(addresses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
