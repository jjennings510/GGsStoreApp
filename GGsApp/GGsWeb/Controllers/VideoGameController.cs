using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GGsWeb.Features;
using GGsWeb.Models;
using GiantBomb.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace GGsWeb.Controllers
{
    public class VideoGameController : Controller
    {
        const string url = Constants.url;
        private User user;
        private readonly IConfiguration config;
        private readonly AlertService alertService;
        public VideoGameController(IConfiguration configuration, AlertService alertService)
        {
            config = configuration;
            this.alertService = alertService;
        }
        /// <summary>
        /// Gets a video game's details
        /// </summary>
        /// <param name="id">ID of the video game you wish to get</param>
        /// <returns>DetailsView</returns>
        public IActionResult Details(int id)
        {
            List<SelectListItem> options = Enumerable.Range(1, 10)
                .Select(n => new SelectListItem
                {
                    Value = n.ToString(),
                    Text = n.ToString()
                }).ToList();
            ViewBag.QuantityOptions = options;
            
            //user = HttpContext.Session.GetObject<User>("User");
            //if (user == null)
            //{
            //    Log.Error("User session was not found");
            //    return RedirectToAction("Login", "Home");
            //}
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync($"videogame/get?id={id}");
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();

                        Log.Information($"Successfully got videogame: {id}");

                        var model = JsonConvert.DeserializeObject<VideoGame>(jsonString.Result);
                        string apikey = config.GetConnectionString("GiantBombAPI");
                        var giantBomb = new GiantBombRestClient(apikey);
                        var game = giantBomb.GetGame(model.apiId);

                        return View(model);
                    }
                }
            }
            Log.Error($"Unsuccessfully got videogame: {id}");
            return View();
        }
    }
}
