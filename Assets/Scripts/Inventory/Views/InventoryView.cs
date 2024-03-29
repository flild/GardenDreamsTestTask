using PocketZone.Extensions;
using PocketZone.Player;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PocketZone.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField]
        private InventorySlotView[] _slots;
        [SerializeField]
        private TMP_Text _textOwner;
        [SerializeField]
        private Button deleteItemButtonPrefab;
        [Inject]
        private InventoryService _inventoryService;
        [Inject]
        private InputReaderSO _input;
        private InventorySlotView _slotForDelete;
        private Button _deleteButton;
        private Vector2Int Size;

        private void Awake()
        {
            Size = _inventoryService.GetInventory(Constants.PlayerInventoryId).Size;
            foreach (var slot in _slots)
            {
                slot.InventorySlotClickEvent += OnSlotClick;
            }
        }
        private void OnEnable()
        {
            _input.MouseClickEvent += OnMouseClick;
        }
        public string OwnerId
        {
            get => _textOwner.text;
            set => _textOwner.text = value;
        }
        public InventorySlotView GetInventorySlotView(int index)
        {
            return _slots[index];
        }
        private void OnSlotClick(InventorySlotView view)
        {
            DeleteCurrentButton();
            if (string.IsNullOrEmpty(view.Title))
                return;
            _deleteButton = Instantiate(deleteItemButtonPrefab, view.transform.position,Quaternion.identity, transform);
            _slotForDelete = view;
            _deleteButton.onClick.AddListener(OnPressDeleteButton);
        }
        private void OnPressDeleteButton()
        {
            var index = 0;
            for (; index < _slots.Length; index++)
            {
                if (_slots[index] == _slotForDelete)
                {
                    break;
                }
            }
            _inventoryService.RemoveItemsFromSlot(
                Constants.PlayerInventoryId,
                new Vector2Int(index / Size.y, index % Size.y),
                _slotForDelete.Title,
                _slotForDelete.Amount);
            DeleteCurrentButton();
        }
        private void DeleteCurrentButton()
        {
            if (_deleteButton != null)
            {
                _deleteButton.onClick.RemoveAllListeners();
                Destroy(_deleteButton.gameObject);
                _slotForDelete = null;
            } 
        }
        private void OnMouseClick(Vector3 position)
        {
            if (_deleteButton == null)
            {
                DeleteCurrentButton();
                return;
            }
            //var worldPosition = Camera.main.ScreenToWorldPoint(position);
            //worldPosition = new Vector2(worldPosition.x, worldPosition.y);
            var distanceToButton = Vector2.Distance(position, _slotForDelete.transform.position);
            if (distanceToButton > 20)
            {
                DeleteCurrentButton();
            }
        }
        private void OnDisable()
        {
            DeleteCurrentButton();
            _input.MouseClickEvent -= OnMouseClick;
        }
        private void OnDestroy()
        {
            foreach (var slot in _slots)
            {
                slot.InventorySlotClickEvent -= OnSlotClick;
            }
        }
    }
}

