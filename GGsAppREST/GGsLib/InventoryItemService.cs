using GGsDB.Models;
using GGsDB.Repos;
using System.Collections.Generic;

namespace GGsLib
{
    public class InventoryItemService : IInventoryItemService
    {
        private IInventoryItemRepo repo;
        public InventoryItemService(IRepo repo)
        {
            this.repo = repo;
        }
        public void AddInventoryItem(InventoryItem item)
        {
            repo.AddInventoryItem(item);
        }
        public void DeleteInventoryItem(int id)
        {
            repo.DeleteInventoryItem(id);
        }
        public List<InventoryItem> GetAllInventoryItemsAtLocation(int locationId)
        {
            return repo.GetAllInventoryItemsAtLocation(locationId);
        }
        public InventoryItem GetInventoryItem(int locationId, int videoGameId)
        {
            return repo.GetInventoryItem(locationId, videoGameId);
        }
        public void UpdateInventoryItem(InventoryItem item)
        {
            repo.UpdateInventoryItem(item);
        }
    }
}