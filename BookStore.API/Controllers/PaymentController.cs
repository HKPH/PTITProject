using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using BookStore.Application.Interface.Service;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/Payment
        [HttpGet]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentService.GetAllAsync();
            return Ok(payments);
        }

        // GET: api/Payment/5
        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
            {
                return NotFound(new { message = "Payment not found." });
            }
            return Ok(payment);
        }

        // POST: api/Payment
        [HttpPost]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPayment = await _paymentService.CreateAsync(paymentDto);
            return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.Id }, createdPayment);
        }

        // PUT: api/Payment/5
        [HttpPut("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> UpdatePayment(int id, [FromBody] PaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _paymentService.UpdateAsync(paymentDto, id);
            if (!updated)
            {
                return NotFound(new { message = "Payment not found." });
            }

            return Ok(new { message = "Payment updated successfully." });
        }

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var deleted = await _paymentService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Payment not found." });
            }

            return Ok(new { message = "Payment deleted successfully." });
        }
    }
}
