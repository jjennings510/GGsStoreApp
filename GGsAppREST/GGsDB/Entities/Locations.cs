using System.Collections.Generic;

namespace GGsDB.Entities
{
    public partial class Locations
    {
        public Locations()
        {
            Inventoryitems = new HashSet<Inventoryitems>();
            Orders = new HashSet<Orders>();
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }

        public virtual ICollection<Inventoryitems> Inventoryitems { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
