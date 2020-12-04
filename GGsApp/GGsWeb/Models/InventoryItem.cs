using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GGsWeb.Models
{
    public class InventoryItem
    {
        public int id { get; set; }
        public int videoGameId { get; set; }
        public VideoGame videoGame { get; set; }
        public int locationId { get; set; }
        public Location location { get; set; }
        [DisplayName("Quantity")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Please select an positive integer")]
        [Range(1, 100, ErrorMessage = "Item quantity must be between 1 and 100")]
        public int quantity { get; set; }
    }
}
