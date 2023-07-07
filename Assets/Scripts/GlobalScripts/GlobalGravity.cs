using System;
using UnityEngine;
using Tools.Singleton;
using UnityEngine.InputSystem;

namespace GlobalScripts
{
    public class GlobalGravity : Singleton<GlobalGravity>
    {
        //invoke when gravity changed
        public event Action OnGravityChanged;
        public Vector3 GravityDirection => _gravityDir;
        public float gravityFactor = 4.9f;

        private Vector3 _gravityDir;
        private const float ColdTime = 0.5f;
        private float _currentTime;
        private PlayerInput _playerInput;

        protected override void Awake()
        {
            base.Awake();

            //initial essential field
            _currentTime = ColdTime;
            _gravityDir = Vector3.down;
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            if (_currentTime < ColdTime)
                _currentTime += Time.deltaTime;
        }

        public void ChangeDirection(InputAction.CallbackContext context)
        {
            if (!context.started || _currentTime < ColdTime) return;

            _gravityDir = context.ReadValue<Vector2>();
            Debug.Log(_gravityDir.ToString());
            OnGravityChanged?.Invoke();
            _currentTime = 0;
        }
    }
}