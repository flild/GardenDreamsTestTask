using System;
using UnityEngine;

namespace Assets.Scripts.Inventory
{
    [Serializable]
    public class ItemData
    {
        public string ItemId;
        public int MaxStackCount;
        public Sprite sprite;
    }
}
