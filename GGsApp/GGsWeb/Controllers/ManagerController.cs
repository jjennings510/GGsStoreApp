using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
    public class ManagerController : Controller
    {
        const string url = Constants.url; 
        private User user;
        private readonly IConfiguration config;
        private readonly AlertService alertService;
        public ManagerController(IConfiguration configuration, AlertService alertService)
        {
            config = configuration;
            this.alertService = alertService;
        }

        /// <summary>
        /// Gets inventory for manager
        /// </summary>
        /// <param name="locationId">ID of the location whose inventory you wan to see</param>
        /// <returns>GetInventory View</returns>
        public IActionResult GetInventory(int locationId)
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
            {
                Log.Error("User session was not found");
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            { 
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    // Get all locations for select list
                    var response = client.GetAsync("location/getAll");
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        var locations = JsonConvert.DeserializeObject<List<Location>>(jsonString.Result);
                        var locationOptions = new List<SelectListItem>();
                        Log.Information("Successfully got locations");
                        foreach (var l in locations)
                        {
                            locationOptions.Add(new SelectListItem { Selected = false, Text = $"{l.city}, {l.state}", Value = l.id.ToString() });
                        }
                        ViewBag.locationOptions = locationOptions;
                    }

                    // Get inventory items at location
                    response = client.GetAsync($"inventoryitem/get/{locationId}");
                    response.Wait();

                    result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        Log.Information($"Successfully got inventory for location: {locationId}");
                        var model = JsonConvert.DeserializeObject<List<InventoryItem>>(jsonString.Result);
                        return View(model);
                    }
                }
            }
            Log.Error("ModelState is invalid for Manager/GetInventory");
            return View();
        }

        /// <summary>
        /// Gets the view for the manager to edit inventory item
        /// </summary>
        /// <param name="locationId">LocationID of the inventory item</param>
        /// <param name="videoGameId">VideoGameID of the inventory item</param>
        /// <returns>EditInventoryItem View</returns>
        [HttpGet]
        public IActionResult EditInventoryItem(int locationId, int videoGameId)
        {
            if (ModelState.IsValid)
            {
                // Get inventory item
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync($"inventoryitem/get/{locationId}/{videoGameId}");
                    response.Wait();
                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();

                        var model = JsonConvert.DeserializeObject<InventoryItem>(jsonString.Result);
                        Log.Information($"Successfully got inventory item: {locationId} - {videoGameId}");
                        return View(model);
                    }
                }
            }
            Log.Error("ModelState is invalid for Manager/EditInventoryItem");
            return View();
        }

        /// <summary>
        /// Updates inventory item in the database
        /// </summary>
        /// <param name="item">Update Inventory Item</param>
        /// <returns>Get Inventory View</returns>
        [HttpPost]
        public IActionResult EditInventoryItem(InventoryItem item)
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
            {
                Log.Error("User session was not found");
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var json = JsonConvert.SerializeObject(item);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PutAsync("inventoryitem/update", data);
                    response.Wait();
                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        alertService.Success("Successfully updated inventory item");
                        Log.Information($"Successfully updated inventory item: {json}");
                        return RedirectToAction("GetInventory", new { locationId = 1 });
                    }
                    alertService.Danger("Something went wrong. Please try again");
                    Log.Error($"Unsuccessfully updated inventory item: {json}");
                }
            }
            Log.Error("ModelState is not valid for Manager/EditInventoryItem");
            return RedirectToAction("GetInventory", new { locationId = 1 });
        }

        /// <summary>
        /// Gets inventory item options using GiantBombAPI
        /// </summary>
        /// <param name="searchString">the name of the video game you are searching</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ViewInventoryItems(string searchString)
        {
            // Get API key:
            string apikey = config.GetConnectionString("GiantBombAPI");
            var giantBomb = new GiantBombRestClient(apikey);
            if (!String.IsNullOrEmpty(searchString))
            {
                var results = giantBomb.SearchForAllGames(searchString);
                Log.Information("Successfully got inventory items");
                return View(results);
            }
            return View(new List<GiantBomb.Api.Model.Game>());
        }

        /// <summary>
        /// Gets the aadd inventory item view
        /// </summary>
        /// <param name="name">The name of the video game you want to add</param>
        /// <param name="id">the GiantBombAPI id of the video game you want to add</param>
        /// <returns>AddInventoryItemView</returns>
        [HttpGet]
        public IActionResult AddInventoryItem(string name, int id)
        {
            if (ModelState.IsValid)
            {
                InventoryItemViewModel model = new InventoryItemViewModel();
                model.name = name;
                model.apiId = id;

                // Get API key:
                string apikey = config.GetConnectionString("GiantBombAPI");
                var giantBomb = new GiantBombRestClient(apikey);
                var game = giantBomb.GetGame(id);
                model.imageURL = game.Image.SmallUrl;
                model.description = game.Description;
                model.description = game.Description;

                // Get Locations
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync("location/getAll");
                    response.Wait();
                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        var locations = JsonConvert.DeserializeObject<List<Location>>(jsonString.Result);
                        var locationOptions = new List<SelectListItem>();
                        foreach (var l in locations)
                        {
                            locationOptions.Add(new SelectListItem { Selected = false, Text = $"{l.street}, {l.city}, {l.state}, {l.zipCode}", Value = l.id.ToString() });
                        }
                        ViewBag.locationOptions = locationOptions;
                    }
                }
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddInventoryItem(InventoryItemViewModel model)
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
            {
                Log.Error("User session was not found");
                return RedirectToAction("Login", "Home");
            }
            // Create Video game
            VideoGame newVideoGame = new VideoGame();
            newVideoGame.name = model.name;
            newVideoGame.platform = model.platform;
            newVideoGame.esrb = model.esrb;
            newVideoGame.cost = model.cost;
            newVideoGame.description = model.description;
            newVideoGame.apiId = model.apiId;
            newVideoGame.imageURL = model.imageURL;

            // Add video game to database
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var json = JsonConvert.SerializeObject(newVideoGame);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync("videogame/add", data);
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    // Successfully added new video game
                    // Get videogame back from db to get id value
                    response = client.GetAsync($"videogame/get/name?name={newVideoGame.name}");
                    response.Wait();
                    result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        newVideoGame = JsonConvert.DeserializeObject<VideoGame>(jsonString.Result);

                        // Create new inventory item and add to DB
                        InventoryItem newItem = new InventoryItem();
                        newItem.locationId = model.locationId;
                        newItem.quantity = model.quantity;
                        newItem.videoGameId = newVideoGame.id;

                        json = JsonConvert.SerializeObject(newItem);
                        data = new StringContent(json, Encoding.UTF8, "application/json");
                        response = client.PostAsync("inventoryitem/add", data);
                        response.Wait();
                        result = response.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            // Successfully added inventory item
                            alertService.Success($"Successfully added {newVideoGame.name} to inventory");
                            return RedirectToAction("GetInventory", "Manager", new { locationId = user.locationId });
                        }
                    }
                }
                // Failed
                alertService.Danger("Something went wrong");
                return RedirectToAction("GetInventory", "Manager", user.locationId);
            }
        }
    }
}
