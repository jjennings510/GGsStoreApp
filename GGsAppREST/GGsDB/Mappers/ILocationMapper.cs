using GGsDB.Entities;
using GGsDB.Models;
using System.Collections.Generic;

namespace GGsDB.Mappers
{
    public interface ILocationMapper
    {
        Location ParseLocation(Locations location);
        Locations ParseLocation(Location location);
        List<Location> ParseLocation(ICollection<Locations> locations);
        ICollection<Locations> ParseLocation(List<Location> locations);

    }
}