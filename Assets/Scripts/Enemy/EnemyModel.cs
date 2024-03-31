using PocketZone.Interfaces;
using PocketZone.Player;
using System;
using System.Collections;
using UnityEngine;


namespace PocketZone.Enemy
{
    //по идее если врагов больше чем 1 вид, то почти всё это надо вынести в базовый абстрактный класс
    public class EnemyModel
    {
 //инит нужен
        private float _health = 15f;
        private float _maxHealth;
        private EnemyView _view;
        public event Action<float, float> HealthChangedEvent;
        public event Action<EnemyModel> UnitDeadEvent;
        private PlayerView _playerView;

        public EnemyModel(EnemyView view)
        {
            _view = view;
            _maxHealth = _health;
            HealthChangedEvent += _view.OnChangeHealthView;
            _view.TakeDamageEvent += TakeDamage;
            _view.PlayerEnterInAttackZone += OnPlayerEnterAttackZone;
            _view.PlayerLeaveInAttackZone += OnPlayerLeaveAttackZone;
        }
        public void TakeDamage(float value)
        {
            _health -= value;

            if (_health <= 0)
            {
                Death();
                return;
            }
            else
                HealthChangedEvent?.Invoke(_health, _maxHealth);
        }
        private void OnPlayerEnterAttackZone(PlayerView player)
        {
            _playerView = player;
            _view.MoveTo(player.transform.position);
        }
        private void OnPlayerLeaveAttackZone(PlayerView player)
        {
            _playerView = null;
            _view.StopMoving();
        }
        public void Death()
        {
            UnitDeadEvent?.Invoke(this);
            GameObject.Destroy(_view.gameObject, 0.1f);
        }
    }
}

