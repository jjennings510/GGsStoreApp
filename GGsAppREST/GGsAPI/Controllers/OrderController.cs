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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("get/location")]
        [Produces("application/json")]
        public IActionResult GetAllOrdersByLocationId(int id)
        {
            try
            {
                return Ok(_orderService.GetAllOrdersByLocationId(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpGet("get/user")]
        [Produces("application/json")]
        public IActionResult GetAllOrdersByUserId(int id)
        {
            try
            {
                return Ok(_orderService.GetAllOrdersByUserId(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpGet("get")]
        [Produces("application/json")]
        public IActionResult GetOrderByDate(DateTime dateTime)
        {
            try
            {
                return Ok(_orderService.GetOrderByDate(dateTime));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpGet("get/{orderId}")]
        [Produces("application/json")]
        public IActionResult GetOrderById(int orderId)
        {
            try
            {
                return Ok(_orderService.GetOrderById(orderId));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPost("add")]
        [Consumes("application/json")]
        public IActionResult AddOrder(Order order)
        {
            try
            {
                _orderService.AddOrder(order);
                return CreatedAtAction("AddOrder", order);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("update")]
        [Consumes("application/json")]
        public IActionResult UpdateOrder(Order order)
        {
            try
            {
                _orderService.UpdateOrder(order);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete")]
        [Consumes("application/json")]
        public IActionResult DeleteOrder(Order order)
        {
            try
            {
                _orderService.DeleteOrder(order);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
