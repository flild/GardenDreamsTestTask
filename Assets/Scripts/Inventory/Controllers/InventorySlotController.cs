using System;
using UnityEngine;

namespace PocketZone.Inventory
{
    public class InventorySlotController
    {
        private readonly InventorySlotView _view;
        private ItemsConfig _itemsConfig;
        //по хорошему вьюшку тоже через интерфейс передавать
        public InventorySlotController(IReadOnlyInventorySlot slot, InventorySlotView view, ItemsConfig itemsConfig)
        {
            _view = view;
            _itemsConfig = itemsConfig;
            slot.ItemIdChanged += OnSlotItemIdChanged;
            slot.ItemAmountChange += OnItemAmountChanged;

            _view.Title = slot.ItemId;
            _view.Amount = slot.Amount;
        }

        private void OnItemAmountChanged(int newAmmount)
        {
            _view.Amount = newAmmount;
        }

        private void OnSlotItemIdChanged(string newItemId)
        {
            _view.Title = newItemId;
            foreach (var item in _itemsConfig.Items)
            {
                if (item.ItemId == newItemId)
                    _view.Sprite = item.sprite;
            }

        }
    }

}
