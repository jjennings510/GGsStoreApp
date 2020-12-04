using GGsDB.Models;
using GGsLib;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GGsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InventoryItemController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;
        public InventoryItemController(IInventoryItemService inventoryItemService)
        {
            _inventoryItemService = inventoryItemService;
        }
        [HttpGet("get/{locationId}")]
        [Produces("application/json")]
        public IActionResult GetAllInventoryItemsAtLocation(int locationId)
        {
            try
            {
                return Ok(_inventoryItemService.GetAllInventoryItemsAtLocation(locationId));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [HttpGet("get/{locationId}/{videoGameId}")]
        [Produces("application/json")]
        public IActionResult GetInventoryItem(int locationId, int videoGameId)
        {
            try
            {
                return Ok(_inventoryItemService.GetInventoryItem(locationId, videoGameId));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPost("add")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult AddInventoryItem(InventoryItem item)
        {
            try
            {
                _inventoryItemService.AddInventoryItem(item);
                return CreatedAtAction("AddInventoryItem", item);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete")]
        public IActionResult DeleteInventoryItem(int id)
        {
            try
            {
                _inventoryItemService.DeleteInventoryItem(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("update")]
        [Consumes("application/json")]
        public IActionResult UpdateInventoryItem(InventoryItem item)
        {
            try
            {
                _inventoryItemService.UpdateInventoryItem(item);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
