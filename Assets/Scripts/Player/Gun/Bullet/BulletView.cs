using PocketZone.Interfaces;
using System;
using UnityEngine;

namespace PocketZone.Player.Gun
{
    public class BulletView: MonoBehaviour
    {
        public event Action<IEnemy, BulletView> HitWithEvent;
        private float _speed;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IEnemy enemy))
                HitWithEvent?.Invoke(enemy, this);
            else
                HitWithEvent?.Invoke(null, this);

        }
        private void Update()
        {
            transform.position += transform.right * Time.deltaTime*_speed;
        }
        public void SetPositionAndRotation(Vector2 pos, Quaternion rot)
        {
            transform.position = pos;
            transform.rotation = rot;
        }
        public void SetSpeed(float value)
        {
            _speed = value;
        }
    }
}

    