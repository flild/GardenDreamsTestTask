
using PocketZone.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Inventory
{
    public class InventoryGrid : IReadOnlyInventoryGrid
    {
        public event Action<Vector2Int> SizeChanged;
        public event Action<string, int> ItemsAdded;
        public event Action<string, int> ItemsRemoved;
        private ItemsConfig _itemsConfig;

        public Vector2Int Size 
        {
            get => new Vector2Int(_data.SizeX, _data.SizeY);
            set
            {
                if (_data.SizeX != value.x || _data.SizeY != value.y)
                {
                    _data.SizeX = value.x;
                    _data.SizeY = value.y;
                    SizeChanged?.Invoke(value);
                }
            }
        }
        public string OwnerId { get => _data.OwnerId; }
        private readonly InventoryGridData _data;
        private readonly Dictionary<Vector2Int, InventorySlot> _slotsMap = new();

        public InventoryGrid(InventoryGridData data, ItemsConfig itemConfig)
        {
            _data = data;
            _itemsConfig = itemConfig;
            var size = new Vector2Int(data.SizeX,data.SizeY);
            for(var i =0; i< size.x; i++)
            {
                for(var j =0; j<size.y; j++)
                {
                    var index = i * size.y + j;
                    var slotData = data.Slots[index];
                    var slot = new InventorySlot(slotData);
                    var position = new Vector2Int(i, j);
                    _slotsMap[position] = slot;
                }
            }
        }

        public AddItemsToInventoryGridResults AddItems(string itemId, int amount = 1)
        {
            var remainingAmount = amount;
            var itemsAddedToSlotWithSameItemsAmount = AddToSlotWithSameItems(itemId, remainingAmount, out remainingAmount);
            
            if (remainingAmount <= 0)
            {
                return new AddItemsToInventoryGridResults(OwnerId, amount, itemsAddedToSlotWithSameItemsAmount);
            }
            var itemsAddedToAvailbleSlotAmount = AddToFirstAvailbleSlots(itemId, remainingAmount, out remainingAmount);
            var totalAddedItemsAmount = itemsAddedToSlotWithSameItemsAmount + itemsAddedToAvailbleSlotAmount;

            return new AddItemsToInventoryGridResults(OwnerId, amount, totalAddedItemsAmount);

        }

        public AddItemsToInventoryGridResults AddItems(Vector2Int slotCord, string itemId, int amount = 1)
        {
            var slot = _slotsMap[slotCord];
            var newValue = slot.Amount + amount;
            var itemsAddedAmount = 0;

            if(slot.IsEmpty)
            {
                slot.ItemId = itemId;
            }

            var itemSlotCapacity = GetItemSlotCapacity(itemId);

            if(newValue > itemSlotCapacity)
            {
                var remainingItems = newValue - itemSlotCapacity;
                var itemsToAddAmount = itemSlotCapacity - slot.Amount;
                itemsAddedAmount += itemsToAddAmount;
                slot.Amount = itemSlotCapacity;

                var result = AddItems(itemId, remainingItems);
                itemsAddedAmount += result.ItemsAddedAmount;
            }
            else
            {
                itemsAddedAmount = amount;
                slot.Amount = newValue;
            }
            ItemsAdded?.Invoke(itemId, amount);
            return new AddItemsToInventoryGridResults(OwnerId, amount, itemsAddedAmount);
        }

        public RemoveItemsFromInventoryResult Remove(string itemId, int amount = 1)
        {
            if (!Has(itemId, amount))
            {
                return new RemoveItemsFromInventoryResult(OwnerId, amount, false);
            }

            var amountToRemove = amount;

            for (var i = 0; i < Size.x; i++)
            {
                for (var j = 0; j < Size.y; j++)
                {
                    var slotCoords = new Vector2Int(i, j);
                    var slot = _slotsMap[slotCoords];

                    if (slot.ItemId != itemId)
                    {
                        continue;
                    }

                    if (amountToRemove > slot.Amount)
                    {
                        amountToRemove -= slot.Amount;
                        Remove(slotCoords, itemId, amountToRemove);

                        if (amountToRemove == 0)
                        {
                            return new RemoveItemsFromInventoryResult(OwnerId, amount, true);
                        }
                    }
                    else
                    {
                        Remove(slotCoords, itemId, amountToRemove);

                        return new RemoveItemsFromInventoryResult(OwnerId, amount, true);
                    }
                }
            }
            throw new Exception("Что-то не так, не удалось удалить предмет" + itemId + ":" + amount);
        }

        public RemoveItemsFromInventoryResult Remove(Vector2Int slotCord, string itemId, int amount = 1)
        {
            var slot = _slotsMap[slotCord];

            if(slot.IsEmpty || slot.ItemId != itemId || slot.Amount < amount)
            {
                return new RemoveItemsFromInventoryResult(OwnerId, amount, false);
            }

            slot.Amount -= amount;

            if(slot.Amount == 0)
            {
                slot.ItemId = null;
            }
            ItemsRemoved?.Invoke(itemId, amount);
            return new RemoveItemsFromInventoryResult(OwnerId, amount, true);
        }

        public int GetAmount(string itemId)
        {
            var amount = 0;
            var slots = _data.Slots;

            foreach(var slot in slots)
            {
                if(slot.ItemId == itemId)
                {
                    amount += slot.Amount;
                }
            }
            return amount;
        }

        public IReadOnlyInventorySlot[,] GetSlots()
        {
            var array = new IReadOnlyInventorySlot[Size.x, Size.y];
            for (var i = 0; i < Size.x; i++)
            {
                for (var j = 0; j < Size.y; j++)
                {
                    var position = new Vector2Int(i, j);
                    array[i, j] = _slotsMap[position];
                }
            }
            return array;
        }
        
        public void SwitchSlots(Vector2Int slotCoordsA, Vector2Int slotCoordB)
        {
            var slotA = _slotsMap[slotCoordsA];
            var slotB = _slotsMap[slotCoordB];
            var tempSlotItemId = slotA.ItemId;
            var tempSlotItemAmount = slotA.Amount;
            slotA.ItemId = slotB.ItemId;
            slotA.Amount = slotB.Amount;
            slotB.ItemId = tempSlotItemId;
            slotB.Amount = tempSlotItemAmount;
        }

        public bool Has(string itemId, int amount)
        {
            var amountExist = GetAmount(itemId);
            return amountExist >= amount;
        }

        private int GetItemSlotCapacity(string itemId)
        {
            foreach(var item in _itemsConfig.Items)
            {
                if (item.ItemId == itemId)
                    return item.MaxStackCount;
            }
            Debug.LogError(string.Concat("Item ", itemId, " was not found in config"));
            return 64;

        }

        private int AddToSlotWithSameItems(string itemId, int amount, out int remainingAmount)
        {
            var itemsAddedAmount = 0;
            remainingAmount = amount;

            for(var i = 0; i < Size.x; i++)
            {
                for(var j = 0; j < Size.y; j++)
                {
                    var coords = new Vector2Int(i, j);
                    var slot = _slotsMap[coords];

                    if(slot.IsEmpty)
                    {
                        continue;
                    }

                    var slotItemCapacity = GetItemSlotCapacity(slot.ItemId);
                    if(slot.Amount >= slotItemCapacity)
                    {
                        continue;
                    }
                    if(slot.ItemId != itemId)
                    {
                        continue;
                    }

                    var newValue = slot.Amount + remainingAmount;

                    if(newValue>slotItemCapacity)
                    {
                        remainingAmount = newValue - slotItemCapacity;
                        var itemsToAddAmount = slotItemCapacity - slot.Amount;
                        itemsAddedAmount += itemsToAddAmount;
                        slot.Amount = slotItemCapacity;

                        if(remainingAmount == 0)
                        {
                            return itemsAddedAmount;
                        }
                    }
                    else
                    {
                        itemsAddedAmount += remainingAmount;
                        slot.Amount = newValue;
                        remainingAmount = 0;

                        return itemsAddedAmount;
                    }
                }
            }
            return itemsAddedAmount;
        }

        private int AddToFirstAvailbleSlots(string itemId, int amount, out int remainingAmount)
        {
            var itemsAddedAmount = 0;
            remainingAmount = amount;

            for(var i = 0; i < Size.x; i++)
            {
                for(var j = 0; j < Size.y; j++)
                {
                    var coords = new Vector2Int(i, j);
                    var slot = _slotsMap[coords];

                    if(!slot.IsEmpty)
                    {
                        continue;
                    }

                    slot.ItemId = itemId;
                    var newValue = remainingAmount;
                    var slotItemCapacity = GetItemSlotCapacity(slot.ItemId);

                    if(newValue>slotItemCapacity)
                    {
                        remainingAmount = newValue - slotItemCapacity;
                        slot.Amount = newValue;
                        remainingAmount = 0;
                        return itemsAddedAmount;
                    }
                    else
                    {
                        itemsAddedAmount += remainingAmount;
                        slot.Amount = newValue;
                        remainingAmount = 0;
                        return itemsAddedAmount;
                    }
                }
            }
            return itemsAddedAmount;
        }
    }
}


