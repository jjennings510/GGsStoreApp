using System.ComponentModel;

namespace GGsWeb.Models
{
    public class CartItem
    {
        public int videoGameId { get; set; }
        public VideoGame videoGame { get; set; }
        [DisplayName("Quantity")]
        public int quantity { get; set; }
    }
}