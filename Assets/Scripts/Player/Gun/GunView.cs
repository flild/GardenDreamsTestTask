using PocketZone.Enemy;
using System;
using UnityEngine;

namespace PocketZone.Player.Gun
{
    public class GunView : MonoBehaviour
    {
        private Transform _target;
        [SerializeField]
        private ShootZoneView _shootZone;
        [SerializeField]
        private BulletView _bulletPrefab;
        public event Action<EnemyView> _EnemyEnterInShootZone;
        public event Action<EnemyView> _EnemyLeaveShootZone;
        public Transform Target
        {
            get => _target;
            set => _target = value;
        }
        public BulletView BulletPrefab
        {
            get => _bulletPrefab;
        }
        private void OnEnable()
        {
            _shootZone._EnemyEnterInTrigger += OnEnemyEnterTrigger;
            _shootZone._EnemyLeaveTrigger += OnEnemyLeaveTrigger;
        }
        private void Update()
        {
            if(Target == null)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                return;
            }
                
            var direction = Target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        private void OnEnemyEnterTrigger(EnemyView enemy)
        {
            _EnemyEnterInShootZone?.Invoke(enemy);
        }
        private void OnEnemyLeaveTrigger(EnemyView enemy)
        {
            _EnemyLeaveShootZone?.Invoke(enemy);
        }
        private void OnDisable()
        {
            _shootZone._EnemyEnterInTrigger -= OnEnemyEnterTrigger;
            _shootZone._EnemyLeaveTrigger -= OnEnemyLeaveTrigger;
        }
    }
}


