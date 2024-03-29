using PocketZone.Extensions;
using PocketZone.Interfaces;
using PocketZone.Inventory;
using PocketZone.Player.Gun;
using System;
using UnityEngine;
using Zenject;

namespace PocketZone.Player
{
    public class PlayerModel: IInitializable,IDisposable, IHaveHpBar
    {
        private PlayerView _view;
        private HealthBarView _healthBar;
        private PlayerMovmentController _movement;
        private GunModel _gun;
        [Inject]
        private InventoryService _inventoryService;
        [Inject]
        private PlayerSO _playerConfig;
        private PlayerData _playerData;

        public event Action<float, float> HealthChangedEvent;
        public event Action PlayerDeadEvent;

        public PlayerModel(PlayerView view)
        {
            _view = view;
        }
        public void Initialize()
        {
            _playerData = _playerConfig.Data;
            _healthBar = _view.GetHealthBarView;

            _movement = new PlayerMovmentController(_view, _playerData.MoveSpeed);
            _gun = new GunModel(_view.GetGunView, 300);

            HealthChangedEvent += _healthBar.OnHealthChange;
            _view.PickItemEvent += PickItem;
            PlayerDeadEvent += _movement.OnPlayerDeath;
            _view.playerTakeDamage += TakeDamage;
        }

        private void PickItem(IPickableItem item)
        {
            _inventoryService.AddItemsToInventory(
                Constants.PlayerInventoryId,
                item.ItemData.ItemId,
                UnityEngine.Random.Range(1, item.ItemData.MaxStackCount));
        }
        private void TakeDamage(float value)
        {
            _playerData.Health -= value;
            HealthChangedEvent?.Invoke(_playerData.Health, _playerData.MaxHealth);
            if (_playerData.Health <= 0)
            {
                Death();
            }
        }
        private void Death()
        {
            Debug.Log("Player dead");
            PlayerDeadEvent?.Invoke();
#if UNITY_EDITOR
            Debug.Break();
#else
            Application.Quit();
#endif

        }

        public void Dispose()
        {
            PlayerDeadEvent -= _movement.OnPlayerDeath;
            _view.PickItemEvent -= PickItem;
            _view.playerTakeDamage -= TakeDamage;
            HealthChangedEvent -= _healthBar.OnHealthChange;
            _gun.Dispose();
        }
    }

}
