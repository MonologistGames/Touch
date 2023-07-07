using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MainLogicalScripts
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputProcessor : MonoBehaviour
    {
        private PlayerInput _playerInput;

        private InputAction _gravityChangeAction;
        private InputAction _floatAction;

        public Vector2 GravityDirection;
        public bool IsFloating;

        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();

            _gravityChangeAction = _playerInput.actions["GravityChange"];
            _floatAction = _playerInput.actions["Float"];
        }

        private void Update()
        {
            GravityDirection = _gravityChangeAction.ReadValue<Vector2>();
            
            _floatAction.started += (content) => IsFloating = true;
            _floatAction.canceled += (content) => IsFloating = false;
        }
    }
}