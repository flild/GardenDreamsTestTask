using System;

namespace PocketZone.Inventory
{
    public interface IReadOnlyInventorySlot
    {
        event Action<string> ItemIdChanged;
        event Action<int> ItemAmountChange;

        string ItemId { get; }
        int Amount { get; }
        bool IsEmpty { get; }
    }

}
