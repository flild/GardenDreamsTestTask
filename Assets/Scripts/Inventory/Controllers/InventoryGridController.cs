using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Inventory
{
    public class InventoryGridController
    {
        private readonly List<InventorySlotController> _slotControllers = new();
        public InventoryGridController(IReadOnlyInventoryGrid inventory, InventoryView view, ItemsConfig itemsConfig)
        {

            var size = inventory.Size;
            var slots = inventory.GetSlots();

            for (var i = 0; i < size.x; i++)
            {
                for (var j = 0; j < size.y; j++)
                {
                    var index = i * size.y + j;
                    var slotView = view.GetInventorySlotView(index);
                    var slot = slots[i, j];
                    _slotControllers.Add(new InventorySlotController(slot, slotView, itemsConfig));
                }
            }
            view.OwnerId = inventory.OwnerId;
        }
    }
}

