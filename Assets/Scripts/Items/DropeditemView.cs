using Assets.Scripts.Inventory;

using PocketZone.Inventory;
using UnityEngine;

namespace PocketZone.Items
{
    public class DropeditemView : MonoBehaviour, IPickableItem
    {
        [SerializeField]
        private SpriteRenderer _sprite;
        [SerializeField]
        private ItemsConfig _itemsConfig;
        [SerializeField]
        private bool _IsNeedToSetData = false;
        [SerializeField]
        private int _ItemIndex;

        public ItemData ItemData { get; private set; }


        private void OnEnable()
        {
            if(!_IsNeedToSetData)
                SetRandomData();
            else
            {
                SetCurrentData();
            }
        }
        public void SetRandomData()
        {
            var randomIndex = Random.Range(0, _itemsConfig.Items.Count);
            ItemData = _itemsConfig.Items[randomIndex];
            _sprite.sprite = _itemsConfig.Items[randomIndex].sprite;
        }
        public void SetCurrentData()
        {
            ItemData = _itemsConfig.Items[_ItemIndex];
            _sprite.sprite = _itemsConfig.Items[_ItemIndex].sprite;
        }

    }
}
