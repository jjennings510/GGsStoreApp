using System.Collections.Generic;

namespace GGsDB.Entities
{
    public partial class Users
    {
        public Users()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public int Locationid { get; set; }
        public virtual Locations Location { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
