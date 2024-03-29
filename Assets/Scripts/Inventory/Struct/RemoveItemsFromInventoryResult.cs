namespace PocketZone.Inventory
{
    public struct RemoveItemsFromInventoryResult
    {
        public readonly string InventoryOwnerId;
        public readonly int ItemsRemoveAmount;
        public readonly bool Success;

        public RemoveItemsFromInventoryResult(string inventoryOwnerId, int itemsToRemoveAmount, bool success)
        {
            InventoryOwnerId = inventoryOwnerId;
            ItemsRemoveAmount = itemsToRemoveAmount;
            Success = success;
        }
    }
}

