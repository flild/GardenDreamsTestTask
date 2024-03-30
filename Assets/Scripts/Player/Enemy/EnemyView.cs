using DG.Tweening;
using PocketZone.Interfaces;
using PocketZone.Items;
using UnityEngine;

namespace PocketZone.Enemy
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyView: MonoBehaviour, IEnemy, IHaveHpBar
    {
        [SerializeField]
        private HealthBarView _healthBarView;
        [SerializeField]
        private float _health = 15f;
        private float _maxHealth;
        [SerializeField]
        private DropeditemView _PickableItemPrefab;
        [SerializeField]
        private int dropItemCount=3;

        public event System.Action<float, float> HealthChangedEvent;

        private void Start()
        {
            HealthChangedEvent += _healthBarView.OnHealthChange;
            _maxHealth = _health;
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
        public void Death()
        {
            for(int i = 0; i < dropItemCount; i++)
            {
                var item = Instantiate(_PickableItemPrefab, transform.position, Quaternion.identity);
                var ranndomPosition = transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1));
                item.transform.DOMove(
                    ranndomPosition
                    , 1f).SetLink(item.gameObject);
            }

            Destroy(gameObject, 0.1f);
        }
        private void OnDisable()
        {
            HealthChangedEvent -= _healthBarView.OnHealthChange;
        }
    }
}

