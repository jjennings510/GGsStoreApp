using GGsDB.Models;
using System.Collections.Generic;

namespace GGsDB.Repos
{
    public interface ILocationRepo
    {
        void AddLocation(Location location);
        void DeleteLocation(int id);
        Location GetLocationById(int id);
        List<Location> GetAllLocations();
        void UpdateLocation(Location location);
    }
}