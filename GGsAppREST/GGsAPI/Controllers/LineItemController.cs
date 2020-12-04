using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GGsDB.Models;
using GGsLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GGsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LineItemController : ControllerBase
    {
        private readonly ILineItemService _lineItemService;
        public LineItemController(ILineItemService lineItemService)
        {
            _lineItemService = lineItemService;
        }
        [HttpGet("get/{orderId}")]
        [Produces("application/json")]
        public IActionResult GetAllLineItems(int orderId)
        {
            try
            {
                return Ok(_lineItemService.GetAllLineItems(orderId));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPost("add")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult AddLineItem(LineItem item)
        {
            try
            {
                _lineItemService.AddLineItem(item);
                return CreatedAtAction("AddLineItem", item);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete")]
        [Consumes("application/json")]
        public IActionResult DeleteLineItem(int id)
        {
            try
            {
                _lineItemService.DeleteLineItem(id);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPut("update")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult UpdateLineItem(LineItem item)
        {
            try
            {
                _lineItemService.UpdateLineItem(item);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
