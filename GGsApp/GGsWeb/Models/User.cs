using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace GGsWeb.Models
{
    public class User
    {
        
        public int id { get; set; }
        [DataType(DataType.Text)]
        [DisplayName("Name")]
        [Required]
        public string name { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage ="Must be a valid email")]
        [DisplayName("Email Address")]
        [Required]
        public string email { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [Required]
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
        public User()
        {
            cart = new Cart();
        }
    }
}
