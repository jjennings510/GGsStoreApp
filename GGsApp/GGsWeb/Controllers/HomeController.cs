using GGsWeb.Features;
using GGsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace GGsWeb.Controllers
{
    public class HomeController : Controller
    {
        private const string url = Constants.url;
        private readonly ILogger<HomeController> _logger;
        public AlertService alertService { get; }

        public HomeController(ILogger<HomeController> logger, AlertService alertService)
        {
            _logger = logger;
            this.alertService = alertService;
        }
        /// <summary>
        /// Gets the Login View
        /// </summary>
        /// <returns>Login View with LoginViewModel</returns>
        [HttpGet]
        public ViewResult Login()
        {
            Log.Information("Attemping to open LoginView");
            var model = new LoginViewModel();
            return View(model);
        }

        /// <summary>
        /// Action called when login button is pressed
        /// </summary>
        /// <param name="model">ViewModel of login credentials</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Log.Information($"Attemping to login user: {model}");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync($"user/get?email={model.email}");
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();

                        var verifiedUser = JsonConvert.DeserializeObject<User>(jsonString.Result);
                        Log.Information($"Succesfully retreived user: {verifiedUser}");
                        if (verifiedUser.password == model.password && verifiedUser.email == model.email)
                        {
                            HttpContext.Session.SetObject("User", verifiedUser);
                            if (verifiedUser.type == Models.User.userType.Customer)
                            {
                                Log.Information("Signing in as Customer");
                                return RedirectToAction("GetInventory", "Customer");
                            }
                            else
                            {
                                Log.Information("Signin in as Manager");
                                return RedirectToAction("GetInventory", "Manager", new { locationId = 1 });
                            }
                        }
                        else
                        { 
                            alertService.Danger("Invalid email or password. Please try again.");
                            Log.Error($"Unsuccessfuly login: {model}");
                            ModelState.AddModelError("Error", "Invalid information");
                            return View(model);
                        }
                    }
                    alertService.Danger("Unable to reach server. Please try again.");
                }
            }
            Log.Error($"ModelState was not valid: {ModelState}");
            return View(model);
        }

        /// <summary>
        /// Signs up the new user and adds to the database
        /// </summary>
        /// <param name="model">SignUpViewModel with the sign in credentials</param>
        /// <returns>Redirects to GetInventory</returns>
        [HttpPost]
        public IActionResult SignUp(SignUpViewModel model)
        {
            Log.Information($"Attempting to sign up model: {model}");
            if (ModelState.IsValid)
            {
                // Verify confirmed password
                if (model.password == model.confirmPassword)
                {
                    // Create and map new user to be added to DB
                    User newUser = new User();
                    newUser.name = model.name;
                    newUser.email = model.email;
                    newUser.locationId = model.locationId;
                    newUser.type = Models.User.userType.Customer;
                    newUser.password = model.password;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(url);

                        // Serialize user
                        var json = JsonConvert.SerializeObject(newUser);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");

                        // Post method to add to db
                        var response = client.PostAsync("user/add", data);
                        response.Wait();

                        var result = response.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var js = result.Content.ReadAsStringAsync();
                            js.Wait();
                            var str = JsonConvert.DeserializeObject<User>(js.Result);
                            // Successful add now get user back from db 
                            response = client.GetAsync($"user/get?email={model.email}");
                            response.Wait();

                            result = response.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                var jsonString = result.Content.ReadAsStringAsync();
                                jsonString.Wait();

                                newUser = JsonConvert.DeserializeObject<User>(jsonString.Result);
                            }


                            HttpContext.Session.SetObject("User", newUser);
                            Log.Information($"Successfully added user: {newUser}");
                            alertService.Success("Succesfully created account!", true);
                            return RedirectToAction("GetInventory", "Customer");
                        }
                    }
                }
                else
                {
                    alertService.Danger("Unable to create account", true);
                    Log.Error("Unsuccesful sign up");
                }
            }
            alertService.Warning("Please fill in all the information");
            return RedirectToAction("SignUp", model);
        }

        /// <summary>
        /// Gets sign up view 
        /// </summary>
        /// <returns>SignUp View</returns>
        [HttpGet]
        public ViewResult SignUp()
        {
            Log.Information("Attemping to get SignUp View");
            var model = new SignUpViewModel();
            
            if (ModelState.IsValid)
            {
                // Get list of locations
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
                            locationOptions.Add(new SelectListItem { Selected = false, Text = $"{l.city}, {l.state}", Value = l.id.ToString() });
                        }
                        model.locationOptions = locationOptions;
                        Log.Information("Succesfully retreived locations");
                    }
                }
                return View(model);
            }
            
            Log.Error($"Model state is invalid: {ModelState.Values}");
            return View();
        }

        /// <summary>
        /// Sign out the current user
        /// </summary>
        /// <returns>LoginView</returns>
        public IActionResult SignOut()
        {
            // Clear session data and redirect to login page
            HttpContext.Session.Clear();
            alertService.Success("Succesfully logged out", true);
            Log.Information("Signing out user");
            return View("Login");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
