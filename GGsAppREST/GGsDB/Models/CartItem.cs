namespace GGsDB.Models
{
    public class CartItem
    {
        public int videoGameId { get; set; }
        public VideoGame videoGame { get; set; }
        public int quantity { get; set; }
    }
}