using GGsDB.Models;
using System.Collections.Generic;

namespace GGsLib
{
    public interface IInventoryItemService
    {
        void AddInventoryItem(InventoryItem item);
        void DeleteInventoryItem(int id);
        List<InventoryItem> GetAllInventoryItemsAtLocation(int locationId);
        InventoryItem GetInventoryItem(int locationId, int videoGameId);
        void UpdateInventoryItem(InventoryItem item);
    }
}