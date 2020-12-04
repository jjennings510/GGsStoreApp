using GGsDB.Models;
using System;
using System.Collections.Generic;

namespace GGsDB.Repos
{
    public interface IOrderRepo
    {
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        Order GetOrderByDate(DateTime dateTime);
        Order GetOrderById(int id);
        List<Order> GetAllOrdersByLocationId(int id);
        List<Order> GetAllOrdersByUserId(int id);
        void DeleteOrder(Order order);
    }
}