using System.Collections.Generic;
namespace GGsDB.Models
{
    public class Cart
    {
        public List<CartItem> cartItems { get; set; }
        public Cart()
        {
            cartItems = new List<CartItem>();
        }
    }
}