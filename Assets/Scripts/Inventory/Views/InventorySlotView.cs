using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace PocketZone.Inventory
{
    public class InventorySlotView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] 
        private TMP_Text _textTitle;
        [SerializeField] 
        private TMP_Text _textAmount;
        [SerializeField] 
        private Image _sprite;
        [SerializeField]
        private Sprite _placeHolder;
        [Inject]
        private ItemsConfig _itemsConfig;
        public event Action<InventorySlotView> InventorySlotClickEvent;

        public string Title
        {
            get => _textTitle.text;
            set
            {

                if(string.IsNullOrEmpty(value))
                {
                    SetSlotEmpty();
                }
                else
                {
                    _textTitle.text = value;
                    SetSprite(value);
                }
            }
        }
        public int Amount
        {
            get => Convert.ToInt32(_textAmount.text);
            set
            {
                if(value <= 0)
                {
                    SetSlotEmpty();
                }
                else
                {
                    _textAmount.text = value.ToString();
                }
            }
        }
        public Sprite Sprite
        {
            get => _sprite.sprite;
            set => _sprite.sprite = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            InventorySlotClickEvent?.Invoke(this);
        }

        public void SetSlotEmpty()
        {
            Sprite = _placeHolder;
            _textAmount.text = "";
            _textTitle.text = "";
        }
        private void SetSprite(string itemId)
        {
            foreach(var item in _itemsConfig.Items)
            {
                if(item.ItemId == itemId)
                {
                    Sprite = item.sprite;
                }
            }
        }

    }

}
