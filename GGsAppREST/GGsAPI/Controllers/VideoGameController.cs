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
    public class VideoGameController : ControllerBase
    {
        private readonly IVideoGameService _videoGameService;
        public VideoGameController(IVideoGameService videoGameService)
        {
            _videoGameService = videoGameService;
        }
        [HttpGet("get")]
        [Produces("application/json")]
        public IActionResult GetVideoGameById(int id)
        {
            try
            {
                return Ok(_videoGameService.GetVideoGameById(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpGet("get/name")]
        [Produces("application/json")]
        public IActionResult GetVideoGameById(string name)
        {
            try
            {
                return Ok(_videoGameService.GetVideoGameByName(name));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpGet("getAll")]
        [Produces("application/json")]
        public IActionResult GetAllVideoGames()
        {
            try
            {
                return Ok(_videoGameService.GetAllVideoGames());
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPost("add")]
        [Consumes("application/json")]
        public IActionResult AddVideoGame(VideoGame videoGame)
        {
            try
            {
                _videoGameService.AddVideoGame(videoGame);
                return CreatedAtAction("AddVideoGame", videoGame);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("update")]
        [Consumes("application/json")]
        public IActionResult UpdateVideoGame(VideoGame videoGame)
        {
            try
            {
                _videoGameService.UpdateVideoGame(videoGame);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete")]
        [Consumes("application/json")]
        public IActionResult DeleteOrder(VideoGame videoGame)
        {
            try
            {
                _videoGameService.DeleteVideoGame(videoGame);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
