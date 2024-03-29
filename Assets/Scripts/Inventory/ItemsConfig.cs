
using Assets.Scripts.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Inventory
{
    [CreateAssetMenu(fileName = "ItemsConfig", menuName = "ScriptableObject/ItemsConfig")]
    public class ItemsConfig : ScriptableObject
    {
        [SerializeField]
        private List<ItemData> _items;

        public List<ItemData> Items
        {
            get { return new List<ItemData>(_items); }
        }
    }
}

