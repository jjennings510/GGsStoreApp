using System;
using System.Collections.Generic;

namespace GGsDB.Entities
{
    public partial class Orders
    {
        public Orders()
        {
            Lineitems = new HashSet<Lineitems>();
        }

        public int Id { get; set; }
        public int Userid { get; set; }
        public int Locationid { get; set; }
        public DateTime Orderdate { get; set; }
        public decimal Totalcost { get; set; }

        public virtual Locations Location { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<Lineitems> Lineitems { get; set; }
    }
}
