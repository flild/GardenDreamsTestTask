using System;


namespace PocketZone.Inventory
{
    public class InventorySlot : IReadOnlyInventorySlot
    {
        public event Action<string> ItemIdChanged;
        public event Action<int> ItemAmountChange;

        public string ItemId
        {
            get => _data.ItemId; 
            set
            {
                if(_data.ItemId != value)
                {
                    _data.ItemId = value;
                    ItemIdChanged?.Invoke(value);
                }
            }
        }
        public int Amount {
            get => _data.Amount;
            set
            {
                if(value != _data.Amount)
                {
                    _data.Amount = value;
                    ItemAmountChange?.Invoke(value);
                }
            }
        }
        public bool IsEmpty => Amount == 0 && string.IsNullOrEmpty(ItemId);

        private readonly InventorySlotData _data;

        public InventorySlot(InventorySlotData data)
        {
            _data = data;
        }
    }
}


