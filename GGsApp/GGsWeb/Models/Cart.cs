using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GGsWeb.Models
{
    public class Cart
    {
        public List<CartItem> cartItems { get; set; }
        [DataType(DataType.Currency)]
        public decimal totalCost { get; set; }
        public Cart()
        {
            cartItems = new List<CartItem>();
        }
    }
}
