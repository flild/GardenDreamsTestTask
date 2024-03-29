using PocketZone.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Inventory
{
    [Serializable]
    public class InventoryGridData
    {
        public string OwnerId;
        public List<InventorySlotData> Slots;
        public int SizeX;
        public int SizeY;
        public InventoryGridData()
        {
            var size = new Vector2Int(5,5);
            var createdInventorySlots = new List<InventorySlotData>();
            var length = size.x*size.y;
            for(var i = 0; i < length; i++)
            {
                createdInventorySlots.Add(new InventorySlotData());
            }
            OwnerId = Constants.PlayerInventoryId;
            SizeX = size.x;
            SizeY = size.y;
            Slots = createdInventorySlots;
        }
    }
}

