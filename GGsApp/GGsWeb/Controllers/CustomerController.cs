using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GGsWeb.Features;
using GGsWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NpgsqlTypes;
using Serilog;

namespace GGsWeb.Controllers
{
    public class CustomerController : Controller
    {
        const string url = "https://localhost:44316/";
        private User user;
        private AlertService alertService { get; }
        public CustomerController(AlertService alertService)
        {
            this.alertService = alertService;
        }

        /// <summary>
        /// Fetches inventory for the user's location ID 
        /// </summary>
        /// <returns>GetInventory View</returns>
        public IActionResult GetInventory()
        {
            Log.Information("Entered Customer Get Inventory Menu");
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
            {
                alertService.Warning("You must be logged in to view inventory");
                Log.Error("User session was not found");
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync($"inventoryitem/get/{user.locationId}");
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        Log.Information($"Succesfully got inventory for {user.name} @ {user.locationId}");
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();

                        var model = JsonConvert.DeserializeObject<List<InventoryItem>>(jsonString.Result);
                        return View(model);
                    }
                }
            }
            Log.Error($"Unsuccessful attempt to get inventory at locationId: {user.locationId}");
            return View();
        }

        /// <summary>
        /// Adds the given item to a user's cart
        /// </summary>
        /// <param name="videoGameId">ID of the video game you want to add</param>
        /// <param name="quantity">Quantity of the item you want to add</param>
        /// <returns>Get Inevntory View</returns>
        public IActionResult AddItemToCart(int videoGameId, int quantity)
        {
            Log.Information($"Attemping to add item to cart: VideoGameID: {videoGameId}, Quantity: {quantity}");
            VideoGame videoGame = new VideoGame();
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
            {
                Log.Error("User session was not found");
                return RedirectToAction("Login", "Home");
            }
            if(ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync($"videogame/get?id={videoGameId}");
                    response.Wait();
                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        var vg = JsonConvert.DeserializeObject<VideoGame>(jsonString.Result);
                        videoGame = vg;
                        Log.Information($"Succesfuly got videogame: {videoGame.id}");
                    }
                }
                CartItem item = new CartItem()
                {
                    videoGameId = videoGameId,
                    videoGame = videoGame,
                    quantity = quantity
                };
                user.cart.totalCost += (videoGame.cost * quantity);
                user.cart.cartItems.Add(item);
                HttpContext.Session.SetObject("User", user);
                Log.Information($"Updated user session data: {user}");
                alertService.Information($"{videoGame.name} added to cart!", true);
                return RedirectToAction("GetInventory");
            }
            Log.Error($"ModelState was not valid: {ModelState}");
            return RedirectToAction("GetInventory");
        }

        /// <summary>
        /// Removes the given item from a user's cart
        /// </summary>
        /// <param name="videoGame">Video game object you wish to remove</param>
        /// <returns>Redirect to GetCart action</returns>
        public IActionResult RemoveItemFromCart(int videoGameId)
        {
            VideoGame vg = new VideoGame();
            Log.Information($"Attempting to remove item from cart: {videoGameId}");
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
                    var response = client.GetAsync($"videogame/get?id={videoGameId}");
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();

                        Log.Information($"Successfully got videogame: {videoGameId}");

                       vg = JsonConvert.DeserializeObject<VideoGame>(jsonString.Result);
                        CartItem item = new CartItem()
                        {
                            videoGameId = videoGameId,
                            videoGame = vg,
                            quantity = 1
                        };
                        user.cart.cartItems.RemoveAll(x => x.videoGameId == item.videoGameId);
                        user.cart.totalCost -= (vg.cost * item.quantity);
                        HttpContext.Session.SetObject("User", user);
                        Log.Information($"Successfully removed item form cart: {videoGameId}");
                        Log.Information($"Updated user session data: {user}");
                        return RedirectToAction("GetCart");
                    }
                }

                
            }
            Log.Error($"ModelState was not valid: {ModelState}");
            return RedirectToAction("GetInventory");
        }

        /// <summary>
        /// Gets the user's cart and presents view
        /// </summary>
        /// <returns>GetCart View</returns>
        public IActionResult GetCart()
        {
            user = HttpContext.Session.GetObject<User>("User");
            Log.Information($"Attempting to get cart for: {user}");
            if (user == null)
            {
                alertService.Warning("You must be logged in to view cart");
                Log.Error("User session was not found");
                return RedirectToAction("Login", "Home");
            }
            return View(user.cart);
        }

        /// <summary>
        /// Gets the EditUser view
        /// </summary>
        /// <returns>EditUserView</returns>
        [HttpGet]
        public IActionResult EditUser()
        {
            user = HttpContext.Session.GetObject<User>("User");
            Log.Information($"Attempting to get information for: {user}");
            if (user == null)
            {
                alertService.Warning("You must be logged in to edit user information");
                Log.Error("User session was not found");
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
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
                        Log.Information($"Succesfully retreived from database: {locations}");
                        foreach (var l in locations)
                        {
                            locationOptions.Add(new SelectListItem { Selected = false, Text = $"{l.city}, {l.state}", Value = l.id.ToString() });
                        }
                        ViewBag.locationOptions = locationOptions;
                        return View(user);
                    }
                    Log.Error("Unsuccessully made API call: location/getAll");
                }
            }
            Log.Error($"ModelState was not valid: {ModelState}");
            return RedirectToAction("GetInventory");
        }

        /// <summary>
        /// Updates user information to the database
        /// </summary>
        /// <param name="newUser">Updated user information</param>
        /// <returns>EditUser View</returns>
        [HttpPost]
        public IActionResult EditUser(User newUser)
        {
            user = HttpContext.Session.GetObject<User>("User");
            Log.Information($"Attempting to get information for: {user}");
            if (user == null)
            {
                Log.Error("User session was not found");
                return RedirectToAction("Login", "Home");
            }
                // Map newUser values
                // User decided not to update password, keep it the same
                if (newUser.password == null)
                    newUser.password = user.password;
                newUser.location = user.location;
                newUser.orders = user.orders;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var json = JsonConvert.SerializeObject(newUser);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PutAsync("user/update", data);
                    response.Wait();
                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        // Successfully edited user
                        HttpContext.Session.SetObject("User", newUser);
                        alertService.Success("Succesfully updated information");
                        Log.Information($"Succesfully updated user: {newUser}");
                        return View(newUser);
                    }
                    else
                    {
                        alertService.Danger("Something went wrong");
                    }
                }
            Log.Error($"ModelState was not valid: {ModelState}");
            return RedirectToAction("GetInventory");
        }
    }
}
