using GGsDB.Entities;
using GGsDB.Models;
using System.Collections.Generic;

namespace GGsDB.Mappers
{
    public interface IInventoryItemMapper
    {
        InventoryItem ParseInventoryItem(Inventoryitems item);
        Inventoryitems ParseInventoryItem(InventoryItem item);
        List<InventoryItem> ParseInventoryItem(ICollection<Inventoryitems> items);
        ICollection<Inventoryitems> ParseInventoryItem(List<InventoryItem> items);
    }
}