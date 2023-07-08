using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Touch.CustomGravity;
using UnityEngine.Serialization;

namespace Touch.Machines.TheObjectController
{
    public class ObjectController : MonoBehaviour
    {
        [Header("Pair")] 
        public GameObject Area;
        public GameObject Obstacle;
        [Header("Basic Settings")] 
        [Min(0f)] public float GravityFactorFactor;
        [Min(0f)] public float VelocityLimit;
        public Vector3 AreaSize;
        [Header("Debug")] public bool IsActiveGravity;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = Obstacle.GetComponent<Rigidbody>();
            
            var tempCollider = Area.GetComponent<BoxCollider>();
            tempCollider.size = AreaSize;
            
            IsActiveGravity = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(147 / 255f, 113 / 255f, 220 / 255f, 0.5f);
            Gizmos.DrawCube(Area.transform.position,AreaSize);
        }

        private void FixedUpdate()
        {
            if (IsActiveGravity) 
                UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            var temp = _rigidbody.velocity;
            temp += GlobalGravity.Instance.Gravity * (GravityFactorFactor * Time.fixedDeltaTime);
            temp = temp.normalized *
                   Mathf.Min(temp.magnitude, VelocityLimit);
            _rigidbody.velocity = temp;
        }
    }
}
