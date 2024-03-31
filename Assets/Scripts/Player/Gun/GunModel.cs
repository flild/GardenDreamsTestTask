using PocketZone.Enemy;
using PocketZone.Extensions;
using PocketZone.Interfaces;
using PocketZone.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace PocketZone.Player.Gun
{
    public class GunModel: IDisposable, IInitializable
    {
        private GunView _gunView;
        private List<EnemyView> _nearEnemyList;
        private EnemyView closestEnemy;
        private int _scanShootZoneDelayms;
        private Task _closestEnemyCalculationTask;
        private bool _IsCalcTask;
        [Inject]
        private InputReaderSO _input;
        [Inject]
        private InventoryService _inventoryService;
        private BulletPool<BulletView> _bulletPool;
        private List<BulletView> _activeBullets = new();
        private GunData _data;
        private const string _bulletId = "Bullets";



        public GunModel(GunView view, int scanShootZoneDelay = 300, InputReaderSO input = null)
        {
            _gunView = view;
            _data= new GunData();
            _scanShootZoneDelayms = scanShootZoneDelay;
            _bulletPool = new BulletPool<BulletView>(_gunView.BulletPrefab);
        }

        public void Initialize()
        {
            _nearEnemyList = new List<EnemyView>();
            _gunView._EnemyEnterInShootZone += OnEnemyEnterInShootZone;
            _gunView._EnemyLeaveShootZone += OnEnemyLeaveShootZone;
            _input.ShootEvent += OnShoot;
            _closestEnemyCalculationTask = StartAsyncClosestEnemyCalculation();
            _data = new GunData();
        }
        private void OnShoot()
        {
            if (_inventoryService.RemoveItems(Constants.PlayerInventoryId, _bulletId).Success)
            {
                var bullet = _bulletPool.GetBullet();
                bullet.HitWithEvent += OnBulletHit;
                bullet.SetPositionAndRotation(_gunView.transform.position, _gunView.transform.rotation);
                bullet.SetSpeed(_data.BulletSpeed);
                _activeBullets.Add(bullet);
            }

        }
        private void OnBulletHit(IEnemy enemy, BulletView bullet)
        {
            if(enemy != null)
            {
                enemy.TakeDamage(_data.Damage);
            }
            bullet.HitWithEvent -= OnBulletHit;
            _activeBullets.Remove(bullet);
            _bulletPool.ReturnBullet(bullet);
        }

        //chat-gpt
        private async Task StartAsyncClosestEnemyCalculation()
        {
            _IsCalcTask = true;
            while (true)
            {
                if (_IsCalcTask == false)
                    break;
                CalculateClosestEnemy();
                await Task.Delay(_scanShootZoneDelayms);
            }
        }
        private void CalculateClosestEnemy()
        {
            //chat-gpt
            closestEnemy = _nearEnemyList.OrderBy(e => Vector3.Distance(e.transform.position, _gunView.transform.position)).FirstOrDefault();
            _gunView.Target = closestEnemy?.transform; 
        }

        private void OnEnemyEnterInShootZone(EnemyView enemy)
        {
            _nearEnemyList.Add(enemy);
        }
        private void OnEnemyLeaveShootZone(EnemyView enemy)
        {
            _nearEnemyList.Remove(enemy);
            if (enemy == closestEnemy)
            {
                closestEnemy = null;
                _gunView.Target = null;
            }
            
        }
        public void Dispose()
        {
            _gunView._EnemyEnterInShootZone -= OnEnemyEnterInShootZone;
            _gunView._EnemyLeaveShootZone -= OnEnemyLeaveShootZone;
            //shat-gpt
            if (_closestEnemyCalculationTask != null && _closestEnemyCalculationTask.IsCompleted)
            {
                _IsCalcTask=false;
                _closestEnemyCalculationTask.Dispose();
            }

        }

    }
}


