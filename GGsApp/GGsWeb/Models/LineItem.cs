using System.ComponentModel;

namespace GGsWeb.Models
{
    public class LineItem
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public Order order { get; set; }
        public int videoGameId { get; set; }
        public VideoGame videoGame { get; set; }
        [DisplayName("Quantity")]
        public int quantity { get; set; }
        public decimal cost { get; set; }
    }
}
