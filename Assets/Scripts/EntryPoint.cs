using PocketZone.Extensions;
using PocketZone.Inventory;
using PocketZone.Player;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace PocketZone
{
    public class EntryPoint : MonoBehaviour
    {
        [Inject]
        private InventoryService _inventoryService;
        [Inject]
        private InputReaderSO _input;
        private GameStateProvider _gameStateProvider;
        private ScreenController _screenController;
        [SerializeField]
        private ScreenView _screenView;
        [SerializeField]
        private ItemsConfig _itemsConfig;
        private bool _IsInventoryOpen = false;
        private void Start()
        {
            _gameStateProvider = new GameStateProvider();
            _gameStateProvider.LoadGameState();
            _input.SaveGameEvent += OnSaveGame;
            _input.OpenInventoryEvent += OnOpenInventory;
            _inventoryService.Registerinventory(_gameStateProvider.GameState.Inventory);
            _screenController = new ScreenController(_inventoryService, _screenView, _itemsConfig);
        }
        private async void OnSaveGame()
        {
            _gameStateProvider.SaveGameStateAsync().Wait();
        }
        private void OnOpenInventory()
        {
            _screenView.gameObject.SetActive(!_IsInventoryOpen);
            _IsInventoryOpen = !_IsInventoryOpen;
            if (_IsInventoryOpen)
                _screenController.OpenInventory(Constants.PlayerInventoryId);
        }
        private void OnDisable()
        {
            _input.SaveGameEvent += OnSaveGame;
            _input.OpenInventoryEvent += OnOpenInventory;
        }
    }
}

