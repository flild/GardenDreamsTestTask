using PocketZone.Enemy;
using System;
using UnityEngine;

namespace PocketZone.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class ShootZoneView : MonoBehaviour
    {
        public event Action<EnemyView> _EnemyEnterInTrigger;
        public event Action<EnemyView> _EnemyLeaveTrigger;
        [SerializeField]
        private Collider2D _enemyTrigger;
        private void OnValidate()
        {
            _enemyTrigger.isTrigger = true;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out EnemyView enemy))
            {
                _EnemyEnterInTrigger?.Invoke(enemy);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out EnemyView enemy))
            {
                _EnemyLeaveTrigger?.Invoke(enemy);
            }
        }
    }
}

