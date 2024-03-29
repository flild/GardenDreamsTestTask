using PocketZone.Player.Gun;
using System;
using UnityEngine;

namespace PocketZone.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField]
        private GunView _gunView;
        [SerializeField]
        private HealthBarView _healtBarView;
        [SerializeField]
        private SpriteRenderer _sprite;
        [SerializeField]
        private Rigidbody2D _rb;
        [SerializeField]
        private Animator _animator;

        public event Action<IPickableItem> PickItemEvent;
        public event Action<float> playerTakeDamage;

        private readonly int _IdAnimMoving = Animator.StringToHash("Moving");

        public void MoveAnimation(Vector2 direction)
        {
            _sprite.flipX = direction.x < 0 ? true : false;
            if (direction.x != 0 || direction.y != 0)
                _animator.SetBool(_IdAnimMoving, true);
            else
            {
                _animator.SetBool(_IdAnimMoving, false);

            } 
        }
        public Rigidbody2D Rigidbody { get => _rb; }
        public GunView GetGunView => _gunView;
        public HealthBarView GetHealthBarView => _healtBarView;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.TryGetComponent(out IPickableItem item))
            {
                PickItemEvent?.Invoke(item);
                Destroy(collision.gameObject);
            }
        }
        public void Takedamage(float value)
        {
            playerTakeDamage?.Invoke(value);
        }
    }
}


