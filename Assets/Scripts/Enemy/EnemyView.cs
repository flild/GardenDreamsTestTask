using PocketZone.Interfaces;
using PocketZone.Items;
using PocketZone.Player;
using System;
using System.Collections;
using UnityEngine;

namespace PocketZone.Enemy
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyView: MonoBehaviour, IEnemy
    {
        [SerializeField]
        private HealthBarView _healthBarView;
        [SerializeField]
        private float _moveSpeed = 3;
        private IEnumerator moveCourutine;
        public event Action<float> TakeDamageEvent;
        public event Action<PlayerView> PlayerEnterInAttackZone;
        public event Action<PlayerView> PlayerLeaveInAttackZone;
        public event Action<float, float> HealthViewChangedEvent;

        private void Start()
        {
            HealthViewChangedEvent += _healthBarView.OnHealthChange;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out PlayerView player))
            {
                PlayerEnterInAttackZone?.Invoke(player);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerView player))
            {
                PlayerLeaveInAttackZone?.Invoke(player);
            }
        }
        public void MoveTo(Vector3 position)
        {
            moveCourutine = Move(position);
            StartCoroutine(moveCourutine);
        }
        public void StopMoving()
        {
            if(moveCourutine != null)
                StopCoroutine(moveCourutine);
        }
        //chat gpt
        private IEnumerator Move(Vector3 position)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = position;
            float t = 0f;
            while (t < 2f)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, t += Time.deltaTime);
                yield return null;
            }
        }
        public void TakeDamage(float value)
        {
            TakeDamageEvent?.Invoke(value);
        }    
        public void OnChangeHealthView(float health, float MaxHealth)
        {
            HealthViewChangedEvent?.Invoke(health, MaxHealth);
        }

        private void OnDisable()
        {
            HealthViewChangedEvent -= _healthBarView.OnHealthChange;
        }
    }
}

