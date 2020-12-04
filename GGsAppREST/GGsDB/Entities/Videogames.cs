using System.Collections.Generic;

namespace GGsDB.Entities
{
    public partial class Videogames
    {
        public Videogames()
        {
            // Cartitems = new HashSet<Cartitems>();
            Inventoryitems = new HashSet<Inventoryitems>();
            Lineitems = new HashSet<Lineitems>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Platform { get; set; }
        public string Esrb { get; set; }
        public string Description { get; set; }
        public int ApiId { get; set; }
        public string ImageURL { get; set; }
        public virtual ICollection<Inventoryitems> Inventoryitems { get; set; }
        public virtual ICollection<Lineitems> Lineitems { get; set; }
    }
}
