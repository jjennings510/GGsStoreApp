using GGsDB.Models;
using GGsDB.Repos;
using System;
using System.Collections.Generic;

namespace GGsLib
{
    public class OrderService : IOrderService
    {
        private IOrderRepo repo;
        public OrderService(IRepo repo)
        {
            this.repo = repo;
        }
        public void AddOrder(Order order)
        {
            repo.AddOrder(order);
        }
        public void DeleteOrder(Order order)
        {
            repo.DeleteOrder(order);
        }
        public List<Order> GetAllOrdersByLocationId(int locationId)
        {
            return repo.GetAllOrdersByLocationId(locationId);
        }
        public List<Order> GetAllOrdersByUserId(int userId)
        {
            return repo.GetAllOrdersByUserId(userId);
        }
        public Order GetOrderByDate(DateTime date)
        {
            return repo.GetOrderByDate(date);
        }
        public Order GetOrderById(int id)
        {
            return repo.GetOrderById(id);
        }
        public void UpdateOrder(Order order)
        {
            repo.UpdateOrder(order);
        }
        /// <summary>
        /// Prepares and completes order while updating appropriate tables in the database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cartService"></param>
        /// <param name="cartItemService"></param>
        /// <param name="videoGameService"></param>
        /// <param name="lineItemService"></param>
        /// <param name="inventoryItemService"></param>
        public Order MakePurchase(User user, VideoGameService videoGameService, LineItemService lineItemService, InventoryItemService inventoryItemService)
        {
            // Create new order object to be added to DB
            Order order = new Order();
            decimal totalCost = 0;
            order.userId = user.id;
            order.locationId = user.locationId;
            order.orderDate = DateTime.Now;
            AddOrder(order);

            // Get that order back with generated id
            Order newOrder = GetOrderByDate(order.orderDate);
            order.id = newOrder.id;

            foreach (var item in user.cart.cartItems)
            {
                // Get video game from user cart and create new line items to be added to DB
                VideoGame videoGame = item.videoGame;
                LineItem lineItem = new LineItem();
                lineItem.orderId = newOrder.id;
                lineItem.videoGameId = item.videoGameId;
                lineItem.cost = videoGame.cost;
                lineItem.quantity = item.quantity;

                totalCost += (videoGame.cost * item.quantity);

                lineItemService.AddLineItem(lineItem);

                // Remove item from inventory
                InventoryItem inventoryItem = inventoryItemService.GetInventoryItem(user.locationId, videoGame.id);
                //inventoryItemService.DiminishInventoryItem(inventoryItem, item.quantity);
                inventoryItem.quantity -= item.quantity;
                inventoryItemService.UpdateInventoryItem(inventoryItem);
            }
            // Clear user cart and update order cost
            user.cart.cartItems.Clear();
            UpdateOrder(order);
            return order;
        }
    }
}