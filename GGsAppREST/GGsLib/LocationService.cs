using GGsDB.Models;
using GGsDB.Repos;
using System.Collections.Generic;

namespace GGsLib
{
    public class LocationService : ILocationService
    {
        private ILocationRepo repo;
        public LocationService(IRepo repo)
        {
            this.repo = repo;
        }
        public void AddLocation(Location location)
        {
            repo.AddLocation(location);
        }
        public void DeleteLocation(int id)
        {
            repo.DeleteLocation(id);
        }
        public List<Location> GetAllLocations()
        {
            return repo.GetAllLocations();
        }
        public Location GetLocationById(int id)
        {
            return repo.GetLocationById(id);
        }
        public void UpdateLocation(Location location)
        {
            repo.UpdateLocation(location);
        }
    }
}