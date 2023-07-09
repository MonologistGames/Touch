using System;
using UnityEngine;
using Tools.Singleton;
namespace Touch.CustomGravity
{
    public class GlobalGravity : Singleton<GlobalGravity>
    {
        public float GravityFactor = 9.8f;
        public event Action<Vector3> OnGravityChanged;
        
        private Vector3 _gravityDir = Vector3.down;
        
        public Vector3 GravityDirection => _gravityDir;
        public Vector3 Gravity => GravityFactor * _gravityDir;
        
        public bool ChangeDirection(Vector3 direction)
        {
            if (_gravityDir == direction) return false;
            
            _gravityDir = direction;
            OnGravityChanged?.Invoke(Gravity);
            return true;
        }

        /// <summary>
        /// Reset global scene gravity direction without triggering
        /// the OnGravityChanged event.
        /// Caution: This method is only used for respawn.
        /// </summary>
        /// <param name="direction">Gravity Direction.</param>
        /// <returns></returns>
        public void ResetGDirection(Vector3 direction)
        {
            _gravityDir = direction;
        }
        
    }
}