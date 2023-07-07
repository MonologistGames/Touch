using System;
using UnityEngine;
using Tools.Singleton;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace GlobalScripts
{
    public class GlobalGravity : Singleton<GlobalGravity>
    {
        //invoke when gravity changed
        public event Action<Vector3> OnGravityChanged;
        public Vector3 GravityDirection => _gravityDir;
        public Vector3 Gravity => GravityFactor * _gravityDir;
        
        public float GravityFactor = 9.8f;
        private Vector3 _gravityDir = Vector3.down;

        public bool ChangeDirection(Vector3 direction)
        {
            if (_gravityDir == direction) return false;
            
            _gravityDir = direction;
            OnGravityChanged?.Invoke(Gravity);
            return true;
        }
    }
}