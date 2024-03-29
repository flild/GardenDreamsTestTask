using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Inventory
{
    [Serializable]
    public class InventorySlotData
    {
        public string ItemId;
        public int Amount;
        public InventorySlotData()
        {
            ItemId = "";
            Amount = 0;
        }
    }

}

