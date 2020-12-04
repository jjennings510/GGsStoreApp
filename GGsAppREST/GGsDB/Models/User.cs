using System.Collections.Generic;

namespace GGsDB.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int locationId { get; set; }
        public Location location { get; set; }
        public userType type { get; set; }
        public List<Order> orders { get; set; }
        public Cart cart { get; set; }
        public enum userType
        {
            Customer,
            Manager
        }
    }
}