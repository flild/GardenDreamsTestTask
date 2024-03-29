using PocketZone.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone
{
    [Serializable]
    public class GameStateData
    {
        public InventoryGridData Inventory;

        public GameStateData()
        {
            Inventory = new InventoryGridData();
        }
    }
}

