using System;
using UnityEngine;
using Zenject;

namespace PocketZone.Player
{
    public class PlayerMovmentController: IInitializable
    {
        private PlayerView _playerView;
        [Inject]
        private InputReaderSO _inputReader;
        private float _speed;

        private Rigidbody2D _rb;
        public PlayerMovmentController(PlayerView playerView, float speed = 20)
        {
            _playerView = playerView;
            _speed = speed;
        }

        public void Initialize()
        {
            _inputReader.MoveEvent += OnMove;
            _rb = _playerView.Rigidbody;
        }
        public void OnPlayerDeath()
        {
            _inputReader.DisableMainMap();
        }
        private void OnMove(Vector2 diriction)
        {
            var _calcDirection = diriction.x*Vector2.right+ diriction.y*Vector2.up;
            _playerView.MoveAnimation(_calcDirection);
            _rb.velocity = _calcDirection;
        }
    }
}


