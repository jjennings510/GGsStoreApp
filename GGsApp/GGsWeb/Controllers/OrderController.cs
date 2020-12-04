using GGsWeb.Features;
using GGsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NpgsqlTypes;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace GGsWeb.Controllers
{
    public class OrderController : Controller
    {
        const string url = Constants.url;
        private User user;

        private AlertService alertService { get; }
        public OrderController(AlertService alertService)
        {
            this.alertService = alertService;
        }

        /// <summary>
        /// Generates the receipt of the order placed
        /// </summary>
        /// <param name="order">the order for receipt</param>
        /// <returns>ReceiptView</returns>
        public IActionResult Receipt(Order order)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync($"order/get/{order.id}");
                    response.Wait();

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var result = response.Result.Content.ReadAsStringAsync();
                        var model = JsonConvert.DeserializeObject<Order>(result.Result);
                        Log.Information($"Successfully got order: {order.id}");
                        return View(model);
                    }
                    Log.Error($"Unsuccessfully got order: {order.id}");
                    return RedirectToAction("GetCart", "Customer");
                }
            }
            Log.Error("Unsuccessfully generated receipt");
            return RedirectToAction("GetCart", "Customer");
        }

        /// <summary>
        /// Adds order to database
        /// </summary>
        /// <param name="cart">The cart of the user who is making the order</param>
        /// <returns>ReceiptView</returns>
        public IActionResult AddOrder(Cart cart)
        {
            // Get User
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
            {
                Log.Error("User session was not found");
                return RedirectToAction("Login", "Home");
            }
            // Ensure model state
            if (ModelState.IsValid)
            {
                // Create new order object and set values
                Order order = new Order();
                order.totalCost = cart.totalCost;
                order.locationId = user.locationId;
                order.orderDate = DateTime.Now;
                NpgsqlDateTime npgsqlDateTime = order.orderDate; // Used for API call
                order.userId = user.id;

                // Set up API calls
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);

                    // Serialize order objecto to be added
                    var json = JsonConvert.SerializeObject(order);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    // Use POST method to add to DB
                    var response = client.PostAsync("order/add", data);
                    response.Wait();

                    while (response.Result.IsSuccessStatusCode)
                    {
                        // Added new order successfully
                        // Now get order we just added to map orderId
                        response = client.GetAsync($"order/get?dateTime={npgsqlDateTime}");
                        response.Wait();
                        var result = response.Result.Content.ReadAsStringAsync();
                        var newOrder = JsonConvert.DeserializeObject<Order>(result.Result);

                        Log.Information($"Successfully created order: {newOrder.id}");
                        // Got order back successfully
                        order.id = newOrder.id;
                        foreach (var item in user.cart.cartItems)
                        {
                            // For each item in cart, map to LineItem object
                            VideoGame videoGame = item.videoGame;
                            LineItem lineItem = new LineItem();
                            lineItem.orderId = order.id;
                            lineItem.videoGameId = item.videoGameId;
                            lineItem.cost = videoGame.cost;
                            lineItem.quantity = item.quantity;

                            // Serialize LineItem object and add to db using POST method
                            json = JsonConvert.SerializeObject(lineItem);
                            data = new StringContent(json, Encoding.UTF8, "application/json");
                            response = client.PostAsync("lineitem/add", data);
                            response.Wait();
                            Log.Information($"Successfully create lineItem: {json}");

                            // Added new line item successfully
                            // Get inventory item and update quantity
                            response = client.GetAsync($"inventoryitem/get/{user.locationId}/{item.videoGameId}");
                            response.Wait();
                            result = response.Result.Content.ReadAsStringAsync();
                            var inventoryItem = JsonConvert.DeserializeObject<InventoryItem>(result.Result);
                            inventoryItem.quantity -= item.quantity;

                            json = JsonConvert.SerializeObject(inventoryItem);
                            data = new StringContent(json, Encoding.UTF8, "application/json");
                            response = client.PutAsync("inventoryitem/update", data);
                            response.Wait();

                            Log.Information($"Successfully updated inventoryItem: {json}");
                        }

                        // Clear cart items and redirect to receipt view
                        user.cart.cartItems.Clear();
                        user.cart.totalCost = 0;
                        HttpContext.Session.SetObject("User", user);
                        return RedirectToAction("Receipt", order);
                    }
                }
            }
            Log.Error("Unable to create order");
            alertService.Warning("Unable to complete order");
            return RedirectToAction("GetInventory", "Customer");
        }

        /// <summary>
        /// Gets the order history customer or location if manager is signed in
        /// </summary>
        /// <param name="sort">the order you want to sort</param>
        /// <param name="locationId">ID of the location (for manager), this will be set to user.locationId for customer</param>
        /// <returns></returns>
        public IActionResult GetOrderHistory(string sort, int locationId)
        {
            ViewBag.SortOptions = new List<SelectListItem>()
            {
                new SelectListItem { Selected = false, Text = "Price (Lowest to Highest)", Value = ("cost_asc")},
                new SelectListItem { Selected = false, Text = "Price (Highest to Lowest)", Value = ("cost_desc")},
                new SelectListItem { Selected = false, Text = "Date (Oldest to Newest)", Value = ("date_asc")},
                new SelectListItem { Selected = false, Text = "Date (Newest to Oldest)", Value = ("date_asc")}
            };
            ViewBag.Locations = new List<Location>();
            // Get User
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
            {
                alertService.Warning("You must be logged in to view order history");
                Log.Error("User session was not found");
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                // Get order history
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    string apiCall = "";

                    if (user.type == Models.User.userType.Customer)
                        apiCall = $"order/get/user?id={user.id}";
                    else
                    {
                        user.locationId = locationId;
                        HttpContext.Session.SetObject("User", user); // work around for url routing. When changing sort order as manager, match locationID
                        apiCall = $"order/get/location?id={locationId}";
                    }

                    var response = client.GetAsync(apiCall);
                    response.Wait();
                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        Log.Information($"Successfully got order history for user: {user.id}");
                        var data = response.Result.Content.ReadAsStringAsync();
                        var model = JsonConvert.DeserializeObject<List<Order>>(data.Result).OrderBy(x => x.id);
                        switch (sort)
                        {
                            case "cost_asc":
                                model = JsonConvert.DeserializeObject<List<Order>>(data.Result).OrderBy(x => x.totalCost);
                                break;
                            case "cost_desc":
                                model = JsonConvert.DeserializeObject<List<Order>>(data.Result).OrderByDescending(x => x.totalCost);
                                break;
                            case "date_asc":
                                model = JsonConvert.DeserializeObject<List<Order>>(data.Result).OrderBy(x => x.orderDate);
                                break;
                            case "date_desc":
                                model = JsonConvert.DeserializeObject<List<Order>>(data.Result).OrderByDescending(x => x.orderDate);
                                break;
                            default:
                                break;
                        }
                        response = client.GetAsync("location/getAll");
                        response.Wait();
                        result = response.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var jsonString = result.Content.ReadAsStringAsync();
                            jsonString.Wait();
                            var locations = JsonConvert.DeserializeObject<List<Location>>(jsonString.Result);
                            ViewBag.Locations = locations;
                            Log.Information("Succesfully got all locations");
                        }
                        return View(model);
                    }
                    Log.Error("Unsuccessfully got order history");
                    if (user.type == Models.User.userType.Customer)
                        return RedirectToAction("GetInventory", "Customer");
                    else
                        return RedirectToAction("GetInventory", "Manager");
                }
            }
            Log.Error("ModelState is invalid for GetOrderHistory");
            if (user.type == Models.User.userType.Customer)
                return RedirectToAction("GetInventory", "Customer");
            else
                return RedirectToAction("GetInventory", "Manager");
        }
    }
}
