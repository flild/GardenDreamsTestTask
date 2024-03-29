using System.Collections.Generic;
using UnityEngine;
using PocketZone.Player.Gun;

namespace PocketZone
{
    //chat-gpt + правки
    public class BulletPool<T> where T : MonoBehaviour
    {
        private int StartBulletCount = 10;
        private int maxBullets = 100;

        private T _bulletPrefab;
        private Transform _parent;
        private Queue<T> bullets = new Queue<T>();

        public BulletPool(T bulletPrefab)
        {
            _bulletPrefab = bulletPrefab;
            _parent = new GameObject("BulletPoolTransform").transform;
            for (int i = 0; i < StartBulletCount; i++)
            {
                CreateBulletInQueue();
            }
        }

        private void CreateBulletInQueue()
        {
            T bullet = GameObject.Instantiate(_bulletPrefab, _parent);
            bullet.gameObject.SetActive(false);
            bullets.Enqueue(bullet);
        }

        public T GetBullet()
        {
            if (bullets.Count > 0)
            {
                T bullet = bullets.Dequeue();
                bullet.gameObject.SetActive(true);
                return bullet;
            }
            else
            {
                CreateBulletInQueue();
                return GetBullet();
            }
        }

        public void ReturnBullet(T bullet)
        {
            bullet.gameObject.SetActive(false);
            bullets.Enqueue(bullet);
        }


    }
}

