using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Inventory
{
    public class InventoryService
    {
        private ItemsConfig _itemConfig;
        public readonly Dictionary<string, InventoryGrid> _inventoriesMap = new();

        public InventoryService(ItemsConfig itemsConfig)
        {
            _itemConfig = itemsConfig;
        }
        public InventoryGrid Registerinventory(InventoryGridData inventorydata)
        {
            var inventory = new InventoryGrid(inventorydata, _itemConfig);
            _inventoriesMap[inventory.OwnerId] = inventory;

            return inventory;
        }
        public AddItemsToInventoryGridResults AddItemsToInventory(string ownerId, string itemId, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerId];
            return inventory.AddItems(itemId, amount);
        }

        public AddItemsToInventoryGridResults AddItemsToInventorySlot(string ownerId, Vector2Int slotCoords, string itemId, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerId];
            return inventory.AddItems(slotCoords, itemId, amount);
        }

        public RemoveItemsFromInventoryResult RemoveItems(string ownerId, string itemId, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerId];
            return inventory.Remove(itemId, amount);
        }

        public RemoveItemsFromInventoryResult RemoveItemsFromSlot(string ownerId, Vector2Int slotCoords, string itemId, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerId];
            return inventory.Remove(slotCoords, itemId, amount);
        }

        public bool Has(string ownerId, string itemId, int amount = 1)
        {
            var inventory = _inventoriesMap[ownerId];
            return inventory.Has(itemId, amount);
        }
        public IReadOnlyInventoryGrid GetInventory(string ownerId)
        {
            return _inventoriesMap[ownerId];
        }


    }

}
