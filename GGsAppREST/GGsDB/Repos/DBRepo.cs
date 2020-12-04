using GGsDB.Entities;
using GGsDB.Mappers;
using GGsDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GGsDB.Repos
{
    /// <summary>
    /// Repository responsible for interfacing directlly with the database
    /// </summary>
    public class DBRepo : IRepo
    {
        private readonly GGsContext context;
        private readonly IMapper mapper;
        public DBRepo(GGsContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        #region User Methods
        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">User you wish to add</param>
        public void AddUser(User user)
        {
            context.Users.Add(mapper.ParseUser(user));
            context.SaveChanges();
        }
        /// <summary>
        /// Updates a user in the database
        /// </summary>
        /// <param name="user">Updated user</param>
        public void UpdateUser(User user)
        {
            //var entity = context.Users.FirstOrDefault(i => i.Id == user.id);
            //if (entity != null)
            //{
            //    entity.Locationid = id;
            //    context.SaveChanges();
            //}
            //return mapper.ParseUser(entity);
            var existingUser = context.Users.Single(x => x.Id == user.id);
            existingUser.Email = user.email;
            existingUser.Locationid = user.locationId;
            existingUser.Name = user.name;
            existingUser.Type = user.type.ToString();
            context.Entry(existingUser).State = EntityState.Modified;
            context.SaveChanges();
        }
        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <returns>A list of all users</returns>
        public List<User> GetAllUsers()
        {
            return mapper.ParseUser(context.Users.Select(x => x).ToList());
        }
        /// <summary>
        /// Gets a single user given an id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>Selected user</returns>
        public User GetUserById(int id)
        {
            User user = new User();
            user = mapper.ParseUser(context.Users.Single(x => x.Id == id));
            user.location = GetLocationById(user.locationId);
            user.orders = GetAllOrdersByUserId(user.id);
            user.cart = new Cart();
            return user;
        }
        /// <summary>
        /// Gets a single user given an email
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <returns>Selected user</returns>
        public User GetUserByEmail(string email)
        {
            User user = new User();
            user = mapper.ParseUser(context.Users.Single(x => x.Email == email));
            user.location = GetLocationById(user.locationId);
            user.orders = GetAllOrdersByUserId(user.id);
            user.cart = new Cart();
            return user;
        }
        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        /// <param name="user">User you wish to delete</param>
        public void DeleteUser(int id)
        {
            var entity = context.Users.Single(u => u.Id == id);
            context.Users.Remove(entity);
            context.SaveChanges();
        }
        #endregion
        #region Inventory Item Methods
        public void AddInventoryItem(InventoryItem item)
        {
            context.Inventoryitems.Add(mapper.ParseInventoryItem(item));
            context.SaveChanges();
        }
        public void DeleteInventoryItem(int id)
        {
            var entity = context.Inventoryitems.Single(i => i.Id == id);
            context.Inventoryitems.Remove(entity);
            context.SaveChanges();
        }
        public List<InventoryItem> GetAllInventoryItemsAtLocation(int locId)
        {
            return mapper.ParseInventoryItem(context.Inventoryitems
                .Include("Videogame")
                .Where(x => x.Locationid == locId)
                .OrderBy(x => x.Id)
                .ToList());

        }
        public InventoryItem GetInventoryItem(int locationId, int videoGameId)
        {
            return mapper.ParseInventoryItem(context.Inventoryitems
                .Include("Videogame")
                .Single(x => x.Locationid == locationId && x.Videogameid == videoGameId));
        }
        public void UpdateInventoryItem(InventoryItem item)
        {
            var existingItem = context.Inventoryitems.Single(x => x.Id == item.id);
            existingItem.Videogameid = item.videoGameId;
            existingItem.Locationid = item.locationId;
            existingItem.Quantity = item.quantity;
            context.Entry(existingItem).State = EntityState.Modified;
            context.SaveChanges();
        }
        #endregion
        #region Line Item Methods
        public void AddLineItem(LineItem item)
        {
            context.Lineitems.Add(mapper.ParseLineItem(item));
            context.SaveChanges();

        }
        public void UpdateLineItem(LineItem item)
        {
            var existingItem = context.Lineitems.Single(x => x.Id == item.id);
            existingItem.Videogameid = item.videoGameId;
            existingItem.Orderid = item.orderId;
            existingItem.Quantity = item.quantity;
            existingItem.Cost = item.cost;
            context.Entry(existingItem).State = EntityState.Modified;
            context.SaveChanges();
        }
        public LineItem GetLineItemById(int id)
        {

            return mapper.ParseLineItem(context.Lineitems.Single(x => x.Id == id));

        }
        public List<LineItem> GetAllLineItems(int orderId)
        {
            return mapper.ParseLineItem(context.Lineitems.Where(x => x.Orderid == orderId).ToList());

        }
        public void DeleteLineItem(int id)
        {
            var entity = context.Lineitems.Single(i => i.Id == id);
            context.Lineitems.Remove(entity);
            context.SaveChanges();
        }
        #endregion
        #region Location Methods
        public void AddLocation(Location location)
        {
            context.Locations.Add(mapper.ParseLocation(location));
            context.SaveChanges();

        }
        public void UpdateLocation(Location location)
        {
            var existingLocation = context.Locations.Single(x => x.Id == location.id);
            existingLocation.Street = location.street;
            existingLocation.City = location.city;
            existingLocation.State = location.state;
            existingLocation.Zipcode = location.zipCode;
            context.Entry(existingLocation).State = EntityState.Modified;
            context.SaveChanges();
        }
        public Location GetLocationById(int id)
        {
            Location location = mapper.ParseLocation(context.Locations.Single(x => x.Id == id));
            location.inventory = GetAllInventoryItemsAtLocation(location.id);
            return location;
        }
        public List<Location> GetAllLocations()
        {   
            List<Location> allLocations = mapper.ParseLocation(context.Locations.Select(x => x).ToList());
            foreach(var location in allLocations)
            {
                location.inventory = GetAllInventoryItemsAtLocation(location.id);
            }
            return allLocations;

        }
        public void DeleteLocation(int id)
        {
            var entity = context.Locations.Single(i => i.Id == id);
            context.Locations.Remove(entity);
            context.SaveChanges();
        }
        #endregion
        #region Order Methods
        public void AddOrder(Order order)
        {
            context.Orders.Add(mapper.ParseOrder(order));
            context.SaveChanges();

        }
        public void DeleteOrder(Order order)
        {
            var entity = context.Orders.Single(u => u.Id == order.id);
            context.Orders.Remove(entity);
            context.SaveChanges();
        }
        public List<Order> GetAllOrdersByLocationId(int locationId)
        {
            List<Order> orders = new List<Order>();
            orders = mapper.ParseOrder(context.Orders
                .Include("Lineitems")
                .Where(x => x.Locationid == locationId)
                .ToList());
            foreach (var order in orders)
            {
                if (order.lineItems != null)
                {
                    foreach (var item in order.lineItems)
                    {
                        item.videoGame = GetVideoGameById(item.videoGameId);
                    }
                }
                order.location = GetLocationById(locationId);
            }
            return orders;
        }
        public List<Order> GetAllOrdersByUserId(int userId)
        {
            List<Order> orders = new List<Order>();
            orders = mapper.ParseOrder(context.Orders
                .Include("Lineitems")
                .Where(x => x.Userid == userId)
                .ToList());
            foreach (var order in orders)
            {
                if (order.lineItems != null)
                {
                    foreach (var item in order.lineItems)
                    {
                        item.videoGame = GetVideoGameById(item.videoGameId);
                    }
                }
                order.location = GetLocationById(order.locationId);
            }
            return orders;
        }
        public Order GetOrderByDate(DateTime orderDate)
        {
            Order order = new Order();
            order = mapper.ParseOrder(context.Orders
                .Include("Lineitems")
                .Single(x => x.Orderdate == orderDate));
            if (order.lineItems != null)
            {
                foreach (var item in order.lineItems)
                {
                    item.videoGame = GetVideoGameById(item.videoGameId);
                }
            }
            order.location = GetLocationById(order.locationId);
            return order;
        }
        public Order GetOrderById(int id)
        {
            Order order = new Order();
            order = mapper.ParseOrder(context.Orders
                .Include("Lineitems")
                .Single(x => x.Id == id));
            if (order.lineItems != null)
            {
                foreach (var item in order.lineItems)
                {
                    item.videoGame = GetVideoGameById(item.videoGameId);
                }
            }
            order.location = GetLocationById(order.locationId);
            return order;

        }
        public void UpdateOrder(Order order)
        {
            var existingOrder = context.Orders.Single(x => x.Id == order.id);
            existingOrder.Locationid = order.locationId;
            existingOrder.Userid = order.userId;
            existingOrder.Orderdate = order.orderDate;
            existingOrder.Totalcost = order.totalCost;
            context.Entry(existingOrder).State = EntityState.Modified;
            context.SaveChanges();
        }
        #endregion
        #region Video Game Methods
        public void AddVideoGame(VideoGame videoGame)
        {
            context.Videogames.Add(mapper.ParseVideoGame(videoGame));
            context.SaveChanges();

        }
        public void UpdateVideoGame(VideoGame videoGame)
        {
            var existingVideoGame = context.Videogames.Single(x => x.Id == videoGame.id);
            existingVideoGame.Name = videoGame.name;
            existingVideoGame.Cost = videoGame.cost;
            existingVideoGame.Platform = videoGame.platform;
            existingVideoGame.Esrb = videoGame.esrb;
            existingVideoGame.Description = videoGame.description;
            context.Entry(existingVideoGame).State = EntityState.Modified;
            context.SaveChanges();
        }
        public VideoGame GetVideoGameById(int id)
        {
            return mapper.ParseVideoGame(context.Videogames.Single(x => x.Id == id));

        }
        public VideoGame GetVideoGameByName(string name)
        {
            return mapper.ParseVideoGame(context.Videogames.First(x => x.Name == name));

        }
        public List<VideoGame> GetAllVideoGames()
        {
            return mapper.ParseVideoGame(context.Videogames.Select(x => x).ToList());

        }
        public void DeleteVideoGame(VideoGame videoGame)
        {
            var entity = context.Videogames.Single(i => i.Id == videoGame.id);
            context.Videogames.Remove(entity);
            context.SaveChanges();
        }
        #endregion
    }
}