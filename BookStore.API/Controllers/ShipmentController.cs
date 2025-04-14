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
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        // GET: api/Shipment
        [HttpGet]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetAllShipments()
        {
            var shipments = await _shipmentService.GetAllAsync();
            return Ok(shipments);
        }

        // GET: api/Shipment/ 
        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> GetShipmentById(int id)
        {
            var shipment = await _shipmentService.GetByIdAsync(id);
            if (shipment == null)
            {
                return NotFound(new { message = "Shipment not found." });
            }
            return Ok(shipment);
        }

        // POST: api/Shipment
        [HttpPost]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> CreateShipment([FromBody] ShipmentDto shipmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdShipment = await _shipmentService.CreateAsync(shipmentDto);
            return CreatedAtAction(nameof(GetShipmentById), new { id = createdShipment.Id }, createdShipment);
        }

        // PUT: api/Shipment/5
        [HttpPut("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> UpdateShipment(int id, [FromBody] ShipmentDto shipmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _shipmentService.UpdateAsync(shipmentDto, id);
            if (!updated)
            {
                return NotFound(new { message = "Shipment not found." });
            }

            return Ok(new { message = "Shipment updated successfully." });
        }

        // DELETE: api/Shipment/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOrAdmin")]

        public async Task<IActionResult> DeleteShipment(int id)
        {
            var deleted = await _shipmentService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Shipment not found." });
            }

            return Ok(new { message = "Shipment deleted successfully." });
        }
    }
}
