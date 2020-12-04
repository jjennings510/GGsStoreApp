namespace GGsDB.Entities
{
    public partial class Inventoryitems
    {
        public int Id { get; set; }
        public int Videogameid { get; set; }
        public int Locationid { get; set; }
        public int Quantity { get; set; }
        public virtual Locations Location { get; set; }
        public virtual Videogames Videogame { get; set; }
    }
}
