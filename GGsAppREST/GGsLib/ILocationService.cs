using GGsDB.Models;
using System.Collections.Generic;

namespace GGsLib
{
    public interface ILocationService
    {
        void AddLocation(Location location);
        void DeleteLocation(int id);
        List<Location> GetAllLocations();
        Location GetLocationById(int id);
        void UpdateLocation(Location location);
    }
}