using DG.Tweening;
using PocketZone.Items;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Enemy
{
    public class EnemyService: MonoBehaviour
    {
        [SerializeField]
        private DropeditemView _PickableItemPrefab;
        [SerializeField]
        private int dropItemCount = 3;
        [SerializeField]
        private EnemyView _enemyPrefab;
        [SerializeField]
        private int _enemyCount = 8;
        private Dictionary<EnemyModel,EnemyView> _enemiesMap = new();
        [SerializeField]
        private Vector2 _size;

        private void Start()
        {
            for(int i = 0; i < _enemyCount; i++)
            {
                SpawnEnemies();
            }
        }
        public void SpawnEnemies()
        {
            var enemyView = Instantiate(_enemyPrefab, GetRandomPosition(), Quaternion.identity, transform);
            var enemy = new EnemyModel(enemyView);
            _enemiesMap.Add(enemy, enemyView);
            enemy.UnitDeadEvent += OnUnitDeath;
        }
        public void OnUnitDeath(EnemyModel enemy)
        {
            enemy.UnitDeadEvent -= OnUnitDeath;
            SpawnDeathDrop(_enemiesMap[enemy]);
            _enemiesMap.Remove(enemy);
        }
        public void SpawnDeathDrop(EnemyView Enemy)
        {
            var item = Instantiate(_PickableItemPrefab, Enemy.transform.position, Quaternion.identity);
            var ranndomPosition = Enemy.transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1));
            for (int i = 0; i < dropItemCount; i++)
            {
                item.transform.DOMove(
                    ranndomPosition
                    , 1f).SetLink(item.gameObject);
            }
        }
        private Vector2 GetRandomPosition()
        {
            var randomX = transform.position.x + Random.Range(-_size.x, _size.x);
            var randomY = transform.position.y + Random.Range(-_size.y, _size.y);
            return new Vector2(randomX, randomY);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //right line
            Gizmos.DrawLine(new Vector2(transform.position.x + _size.x, transform.position.y + _size.y),
                new Vector2(transform.position.x + _size.x, transform.position.y - _size.y));
            //left line
            Gizmos.DrawLine(new Vector2(transform.position.x - _size.x, transform.position.y + _size.y),
                new Vector2(transform.position.x - _size.x, transform.position.y - _size.y));
            //up line
            Gizmos.DrawLine(new Vector2(transform.position.x - _size.x, transform.position.y + _size.y),
                new Vector2(transform.position.x + _size.x, transform.position.y + _size.y));
            //bot line
            Gizmos.DrawLine(new Vector2(transform.position.x - _size.x, transform.position.y - _size.y),
                new Vector2(transform.position.x + _size.x, transform.position.y - _size.y));

        }
    }
}

