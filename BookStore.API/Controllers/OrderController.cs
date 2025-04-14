using BookStore.Application.Dtos;
using BookStore.Application.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Order
        [HttpGet]
        //[Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetAllOrders(int page = 1, int pageSize = 10, string? searchTerm = null, string sortBy = "OrderDate", bool sortDirection = false)
        {
            var orders = await _orderService.GetAllAsync(page, pageSize, searchTerm, sortBy, sortDirection);

            return Ok(orders);
        }

        // GET: api/Order/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetOrdersByUserId(int userId, [FromQuery] int status = -1)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId, status);
            return Ok(orders);
        }


        // GET: api/Order/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // POST: api/Order
        [HttpPost]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createdOrderDto)
        {
            try
            {
                var orderDto = createdOrderDto.Order;
                var shipmentDto = createdOrderDto.Shipment;
                var paymentDto = createdOrderDto.Payment;

                var createdOrder = await _orderService.CreateAsync(orderDto, shipmentDto, paymentDto);

                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // PUT: api/Order/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> UpdateOrderStatus(int id, [FromQuery] int status)
        {
            try
            {
                var result = await _orderService.UpdateStatusAsync(status, id);
                if (!result)
                {
                    return BadRequest(new { Message = "Update failed." });
                }
                return Ok(new { Message = "Order updated successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await _orderService.DeleteAsync(id);
                if (!result)
                {
                    return BadRequest(new { Message = "Delete failed." });
                }
                return Ok(new { Message = "Order deleted successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
