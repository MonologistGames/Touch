using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Touch.Machines.TheObjectController;
using Touch.CustomGravity;

namespace Touch.Machines.Waterwheel
{
    public class WaterWheelController : MonoBehaviour
    {
        [Header("Pair")] 
        public GameObject Area;
        public GameObject Water;
        [Header("Basic Settings")] 
        [Min(0f)] public float GravityFactorFactor;
        [Min(0f)] public float VelocityLimit;
        public Vector3 AreaSize;
        [Header("Debug")] public bool IsActiveGravity;

        private Rigidbody _rigidbody;
        private Vector3 _waterDefaultPosition;

        private void Awake()
        {
            _waterDefaultPosition=Water.transform.position;
        }

        private void Start()
        {
            _rigidbody = Water.GetComponent<Rigidbody>();
            
            var tempCollider = Area.GetComponent<BoxCollider>();
            tempCollider.size = AreaSize;
            
            GlobalGravity.Instance.OnGravityChanged += ResetPosition;
            
            IsActiveGravity = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(140 / 255f, 190 / 255f, 220 / 255f, 0.5f);
            Gizmos.DrawCube(Area.transform.position,AreaSize);
            Gizmos.DrawSphere(Water.transform.position,Water.GetComponent<SphereCollider>().radius);
        }

        private void FixedUpdate()
        {
            if (IsActiveGravity) 
                UpdateVelocity();
            else
            {
                Water.transform.position = _waterDefaultPosition;
            }
        }

        private void UpdateVelocity()
        {
            var temp = _rigidbody.velocity;
            temp += GlobalGravity.Instance.Gravity * (GravityFactorFactor * Time.fixedDeltaTime);
            temp = temp.normalized *
                   Mathf.Min(temp.magnitude, VelocityLimit);
            _rigidbody.velocity = temp;
        }
        
        public void ResetPosition(Vector3 gravity)
        {
            Water.transform.position = _waterDefaultPosition;
            _rigidbody.velocity=Vector3.zero;
        }
    }
}

