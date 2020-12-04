using GGsDB.Entities;
using GGsDB.Mappers;
using GGsDB.Models;
using GGsDB.Repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GGsTest.GGsDBTest
{

    public class DBRepoTest
    {
        DBRepo repo;
        readonly DBMapper mapper = new DBMapper();
        private readonly Location testLocation = new Location()
        {
            id = 10,
            street = "772 Bat Dr",
            city = "San Jose",
            state = "CA",
            zipCode = "94541"
        };

        private readonly User testUser = new User()
        {
            id = 1,
            name = "Jacob",
            email = "jjj@gmail.com",
            locationId = 1,
            type = User.userType.Customer
        };

        private readonly Order testOrder = new Order()
        {
            id = 1,
            userId = 1,
            locationId = 1,
            orderDate = DateTime.Now,
            totalCost = 59.99m
        };

        private readonly VideoGame testGame = new VideoGame()
        {
            id = 1,
            apiId = 1,
            cost = 59.99m,
            description = "This game is great",
            esrb = "M",
            imageURL = "",
            platform = "PS5",
            name = "Cyberpunk 2077"
        };

        private readonly InventoryItem testInventoryItem = new InventoryItem()
        {
            id = 1,
            locationId = 1,
            quantity = 1,
            videoGameId = 1
        };

        private readonly LineItem testLineItem = new LineItem()
        {
            id = 1,
            videoGameId = 1,
            quantity = 1,
            cost = 59.99m,
            orderId = 1
        };

        public void SeedData(GGsContext testContext)
        {
            testContext.Users.AddRange(mapper.ParseUser(testUser));
            testContext.Locations.AddRange(mapper.ParseLocation(testLocation));
            testContext.Inventoryitems.AddRange(mapper.ParseInventoryItem(testInventoryItem));
            testContext.Lineitems.AddRange(mapper.ParseLineItem(testLineItem));
            testContext.Orders.AddRange(mapper.ParseOrder(testOrder));
            testContext.Videogames.AddRange(mapper.ParseVideoGame(testGame));
        }

        [Fact]
        public void AddUserShouldAdd()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("AddUserShouldAdd").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);
            testContext.Locations.AddRange(mapper.ParseLocation(testLocation));

            // Act
            repo.AddUser(testUser);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Users.Single(u => u.Name == testUser.name));
        }

        [Fact]
        public void GetUserByEmailShoudlGet()
        {
            // Arange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetUserByEmailShoudlGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);
            testContext.Locations.AddRange(mapper.ParseLocation(testLocation));

            // Act
            repo.AddUser(testUser);
            User getUser = repo.GetUserByEmail(testUser.email);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Users.Single(u => u.Id == getUser.id));
        }

        [Fact]
        public void GetUserByIdShouldGet()
        {
            // Arange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetUserByIdShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);
            testContext.Locations.AddRange(mapper.ParseLocation(testLocation));

            // Act
            repo.AddUser(testUser);
            User getUser = repo.GetUserById(testUser.id);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Users.Single(u => u.Name == getUser.name));
        }

        [Fact]
        public void AddLocationShouldAdd()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("AddLocationShouldAdd").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddLocation(testLocation);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Locations.Single(u => u.Street == testLocation.street));
        }
        [Fact]
        public void GetAllLocationsShouldGet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetAllLocationsShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddLocation(testLocation);
            List<Location> newLocation = repo.GetAllLocations();

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(newLocation);
        }
        [Fact]
        public void AddOrderShouldAdd()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("AddOrderShouldAdd").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddOrder(testOrder);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Orders.Single(u => u.Orderdate == testOrder.orderDate));
        }
        [Fact]
        public void GetOrderByDateShouldGet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetOrderByDateShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);
            testContext.Locations.AddRange(mapper.ParseLocation(testLocation));

            // Act
            repo.AddOrder(testOrder);
            Order newOrder = repo.GetOrderByDate(testOrder.orderDate);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Orders.Single(u => u.Id == newOrder.id));
        }
        [Fact]
        public void GetOrderByIdShouldGet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetOrderByIdShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);
            testContext.Locations.AddRange(mapper.ParseLocation(testLocation));

            // Act
            repo.AddOrder(testOrder);
            Order newOrder = repo.GetOrderById(testOrder.id);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Orders.Single(u => u.Id == newOrder.id));
        }
        [Fact]
        public void GetAllOrdersByLocationIdShouldGet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetAllOrdersByLocationIdShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);
            testContext.Locations.AddRange(mapper.ParseLocation(testLocation));

            // Act
            repo.AddOrder(testOrder);
            List<Order> newOrder = repo.GetAllOrdersByLocationId(testOrder.locationId);
            int count = 1;

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.Equal(newOrder.Count, count);
        }
        [Fact]
        public void GetAllOrdersByUserIdShouldGet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetAllOrdersByUserIdShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);
            testContext.Locations.AddRange(mapper.ParseLocation(testLocation));

            // Act
            repo.AddOrder(testOrder);
            List<Order> newOrder = repo.GetAllOrdersByUserId(testOrder.userId);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(newOrder);
        }
        [Fact]
        public void GetVideoGameByIdShouldGet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetVideoGameByIdShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddVideoGame(testGame);
            VideoGame newGame = repo.GetVideoGameById(testGame.id);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Videogames.Single(u => u.Id == newGame.id));
        }
        [Fact]
        public void GetVideoGameByNameShouldGet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetVideoGameByNameShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddVideoGame(testGame);
            VideoGame newGame = repo.GetVideoGameByName(testGame.name);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Videogames.Single(u => u.Id == newGame.id));
        }
        [Fact]
        public void GetAllVideoGamesShouldGet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetAllVideoGamesShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddVideoGame(testGame);
            List<VideoGame> newGame = repo.GetAllVideoGames();

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(newGame);
        }
        [Fact]
        public void AddVideoGameShouldAdd()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("AddVideoGameShouldAdd").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddVideoGame(testGame);
            

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Videogames.Single(u => u.Id == testGame.id));
        }
        [Fact]
        public void AddLineItemShouldAdd()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("AddLineItemShouldAdd").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddLineItem(testLineItem);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Lineitems.Single(u => u.Id == testLineItem.id));
        }
        [Fact]
        public void GetLineItemByIdShouldGet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetLineItemByIdShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddLineItem(testLineItem);
            LineItem newItem = repo.GetLineItemById(testLineItem.id);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Lineitems.Single(u => u.Id == newItem.id));
        }
        [Fact]
        public void GetAllLineItemsShouldGet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetAllLineItemsShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddLineItem(testLineItem);
            List<LineItem> newItem = repo.GetAllLineItems(testLineItem.orderId);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(newItem);
        }
        [Fact]
        public void AddInventoryItemShouldAdd()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("AddInventoryItemShouldAdd").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddInventoryItem(testInventoryItem);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Inventoryitems.Single(u => u.Id == testInventoryItem.id));
        }
        [Fact]
        public void GetInventoryItemShouldGet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetInventoryItemShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);
            testContext.Videogames.AddRange(mapper.ParseVideoGame(testGame));

            // Act
            repo.AddInventoryItem(testInventoryItem);
            var newItem = repo.GetInventoryItem(testInventoryItem.locationId, testInventoryItem.videoGameId);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(assertContext.Inventoryitems.Single(u => u.Id == newItem.id));
        }
        [Fact]
        public void GetAllInventoryItemsAtLocationShouldGet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<GGsContext>().UseInMemoryDatabase("GetAllInventoryItemsAtLocationShouldGet").Options;
            using var testContext = new GGsContext(options);
            repo = new DBRepo(testContext, mapper);

            // Act
            repo.AddInventoryItem(testInventoryItem);
            var newItem = repo.GetAllInventoryItemsAtLocation(testInventoryItem.locationId);

            // Assert
            using var assertContext = new GGsContext(options);
            Assert.NotNull(newItem);
        }
    }
}