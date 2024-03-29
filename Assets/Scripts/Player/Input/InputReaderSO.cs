using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PocketZone.Player
{
    [CreateAssetMenu(fileName = "InputReadear", menuName = "ScriptableObject/Player/InputReader")]
    public class InputReaderSO : ScriptableObject, InputMap.IMainMapActions
    {
        private InputMap _gameInput;
        public event Action<Vector2> MoveEvent;
        public event Action ShootEvent;
        public event Action OpenInventoryEvent;
        public event Action SaveGameEvent;
        public event Action<Vector3> MouseClickEvent;

        private void OnEnable()
        {
            _gameInput ??= new InputMap();
            _gameInput.MainMap.SetCallbacks(this);
            EnableMainMap();
        }
        public void EnableMainMap()
        {
            _gameInput.Enable();
        }
        public void DisableMainMap()
        {
            _gameInput.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                ShootEvent?.Invoke();
        }

        public void OnOpenInventory(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                OpenInventoryEvent?.Invoke();
        }

        public void OnSave(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                SaveGameEvent?.Invoke();
        }

        public void OnLeftMouseClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                
                MouseClickEvent?.Invoke(Input.mousePosition);
            }
                
        }
    }
}


