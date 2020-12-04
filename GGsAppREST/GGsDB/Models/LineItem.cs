namespace GGsDB.Models
{
    public class LineItem
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public Order order { get; set; }
        public int videoGameId { get; set; }
        public VideoGame videoGame { get; set; }
        public int quantity { get; set; }
        public decimal cost { get; set; }
    }
}