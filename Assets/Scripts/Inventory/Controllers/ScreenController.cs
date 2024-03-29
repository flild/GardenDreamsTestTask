using UnityEngine;

namespace PocketZone.Inventory
{
    public class ScreenController
    {
        private readonly ScreenView _view;
        private readonly InventoryService _inventoryService;
        private ItemsConfig _itemsConfig;
        private InventoryGridController _currentInventoryController;

        public ScreenController(InventoryService inventoryService, ScreenView view, ItemsConfig itemsConfig)
        {
            _inventoryService = inventoryService;
            _view = view;
            _itemsConfig = itemsConfig;
        }

        public void OpenInventory(string OwnerId)
        {
            var inventory = _inventoryService.GetInventory(OwnerId);
            var invetoryView = _view.InventoryView;

            _currentInventoryController = new InventoryGridController(inventory, invetoryView, _itemsConfig);
        }
    }
}


