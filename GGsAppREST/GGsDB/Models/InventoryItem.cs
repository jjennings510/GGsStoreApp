namespace GGsDB.Models
{
    public class InventoryItem
    {
        public int id { get; set; }
        public int videoGameId { get; set; }
        public VideoGame videoGame { get; set; }
        public int locationId { get; set; }
        public Location location { get; set; }
        public int quantity { get; set; }
    }
}