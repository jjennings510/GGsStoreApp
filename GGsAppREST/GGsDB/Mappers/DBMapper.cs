using GGsDB.Entities;
using GGsDB.Models;
using System.Collections.Generic;

namespace GGsDB.Mappers
{
    public class DBMapper : IMapper
    {
        public InventoryItem ParseInventoryItem(Inventoryitems item)
        {
            return new InventoryItem()
            {
                id = item.Id,
                videoGameId = item.Videogameid,
                locationId = item.Locationid,
                quantity = item.Quantity,
                videoGame = ParseVideoGame(item.Videogame)
            };
        }

        public Inventoryitems ParseInventoryItem(InventoryItem item)
        {
            return new Inventoryitems()
            {
                Videogameid = item.videoGameId,
                Locationid = item.locationId,
                Quantity = item.quantity
            };
        }

        public List<InventoryItem> ParseInventoryItem(ICollection<Inventoryitems> items)
        {
            if (items.Equals(null))
                return new List<InventoryItem>();
            List<InventoryItem> allItems = new List<InventoryItem>();
            foreach (var item in items)
            {
                allItems.Add(ParseInventoryItem(item));
            }
            return allItems;
        }

        public ICollection<Inventoryitems> ParseInventoryItem(List<InventoryItem> items)
        {
            if (items.Equals(null))
                return new List<Inventoryitems>();
            ICollection<Inventoryitems> allItems = new List<Inventoryitems>();
            foreach (var item in items)
            {
                allItems.Add(ParseInventoryItem(item));
            }
            return allItems;
        }

        public LineItem ParseLineItem(Lineitems item)
        {
            return new LineItem()
            {
                id = item.Id,
                orderId = item.Orderid,
                videoGameId = item.Videogameid,
                quantity = item.Quantity,
                cost = item.Cost
            };
        }

        public Lineitems ParseLineItem(LineItem item)
        {
            return new Lineitems()
            {
                Orderid = item.orderId,
                Videogameid = item.videoGameId,
                Quantity = item.quantity,
                Cost = item.cost
            };
        }

        public List<LineItem> ParseLineItem(ICollection<Lineitems> items)
        {
            if (items.Equals(null))
                return new List<LineItem>();
            List<LineItem> allItems = new List<LineItem>();
            foreach (var item in items)
            {
                allItems.Add(ParseLineItem(item));
            }
            return allItems;
        }

        public ICollection<Lineitems> ParseLineItem(List<LineItem> items)
        {
            if (items.Equals(null))
                return new List<Lineitems>();
            ICollection<Lineitems> allItems = new List<Lineitems>();
            foreach (var item in items)
            {
                allItems.Add(ParseLineItem(item));
            }
            return allItems;
        }

        public Location ParseLocation(Locations location)
        {
            return new Location()
            {
                id = location.Id,
                street = location.Street,
                city = location.City,
                state = location.State,
                zipCode = location.Zipcode
            };
        }

        public Locations ParseLocation(Location location)
        {
            return new Locations()
            {
                Street = location.street,
                City = location.city,
                State = location.state,
                Zipcode = location.zipCode,
            };
        }

        public List<Location> ParseLocation(ICollection<Locations> locations)
        {
            if (locations.Equals(null))
                return new List<Location>();
            List<Location> allLocations = new List<Location>();
            foreach (var l in locations)
            {
                allLocations.Add(ParseLocation(l));
            }
            return allLocations;
        }

        public ICollection<Locations> ParseLocation(List<Location> locations)
        {
            if (locations.Equals(null))
                return new List<Locations>();
            ICollection<Locations> allLocations = new List<Locations>();
            foreach (var l in locations)
            {
                allLocations.Add(ParseLocation(l));
            }
            return allLocations;
        }

        public Order ParseOrder(Orders order)
        {
            return new Order()
            {
                id = order.Id,
                userId = order.Userid,
                locationId = order.Locationid,
                orderDate = order.Orderdate,
                totalCost = order.Totalcost,
                lineItems = ParseLineItem(order.Lineitems)
            };
        }

        public Orders ParseOrder(Order order)
        {
            return new Orders()
            {
                Userid = order.userId,
                Locationid = order.locationId,
                Orderdate = order.orderDate,
                Totalcost = order.totalCost
            };
        }

        public List<Order> ParseOrder(ICollection<Orders> orders)
        {
            if (orders.Equals(null))
                return new List<Order>();
            List<Order> allOrders = new List<Order>();
            foreach (var o in orders)
            {
                allOrders.Add(ParseOrder(o));
            }
            return allOrders;
        }

        public ICollection<Orders> ParseOrder(List<Order> orders)
        {
            if (orders.Equals(null))
                return new List<Orders>();
            ICollection<Orders> allOrders = new List<Orders>();
            foreach (var o in orders)
            {
                allOrders.Add(ParseOrder(o));
            }
            return allOrders;
        }

        public User ParseUser(Users user)
        {
            if (user.Type.Equals("Customer"))
                return new User()
                {
                    id = user.Id,
                    name = user.Name,
                    email = user.Email,
                    locationId = user.Locationid,
                    type = User.userType.Customer,
                    password = user.Password
                };
            else
                return new User()
                {
                    id = user.Id,
                    name = user.Name,
                    email = user.Email,
                    locationId = user.Locationid,
                    type = User.userType.Manager,
                    password = user.Password
                };
        }

        public Users ParseUser(User user)
        {
            return new Users()
            {
                Name = user.name,
                Email = user.email,
                Locationid = user.locationId,
                Type = user.type.ToString(),
                Password = user.password
            };
        }

        public List<User> ParseUser(ICollection<Users> users)
        {
            if (users.Equals(null))
                return new List<User>();
            List<User> allUsers = new List<User>();
            foreach (var u in users)
            {
                allUsers.Add(ParseUser(u));
            }
            return allUsers;
        }

        public ICollection<Users> ParseUser(List<User> users)
        {
            if (users.Equals(null))
                return new List<Users>();
            ICollection<Users> allUsers = new List<Users>();
            foreach (var u in users)
            {
                allUsers.Add(ParseUser(u));
            }
            return allUsers;
        }

        public VideoGame ParseVideoGame(Videogames videogame)
        {
            return new VideoGame()
            {
                id = videogame.Id,
                name = videogame.Name,
                cost = videogame.Cost,
                platform = videogame.Platform,
                esrb = videogame.Esrb,
                description = videogame.Description,
                apiId = videogame.ApiId,
                imageURL = videogame.ImageURL
            };
        }

        public Videogames ParseVideoGame(VideoGame videogame)
        {
            return new Videogames()
            {
                Name = videogame.name,
                Cost = videogame.cost,
                Platform = videogame.platform,
                Esrb = videogame.esrb,
                Description = videogame.description,
                ApiId = videogame.apiId,
                ImageURL = videogame.imageURL
            };
        }

        public List<VideoGame> ParseVideoGame(ICollection<Videogames> videogames)
        {
            if (videogames.Equals(null))
                return new List<VideoGame>();
            List<VideoGame> allVideoGames = new List<VideoGame>();
            foreach (var vg in videogames)
            {
                allVideoGames.Add(ParseVideoGame(vg));
            }
            return allVideoGames;
        }

        public ICollection<Videogames> ParseVideoGame(List<VideoGame> videogames)
        {
            if (videogames.Equals(null))
                return new List<Videogames>();
            ICollection<Videogames> allVideoGames = new List<Videogames>();
            foreach (var vg in videogames)
            {
                allVideoGames.Add(ParseVideoGame(vg));
            }
            return allVideoGames;
        }
    }
}