using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Touch.CustomGravity;
using Touch.PlayerLife;

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
        private Vector3 _obstacleOriginPosition;

        private void Awake()
        {
            _rigidbody = Obstacle.GetComponent<Rigidbody>();
            
            var tempCollider = Area.GetComponent<BoxCollider>();
            tempCollider.size = AreaSize;
            
            IsActiveGravity = false;
        }
        private void Start()
        {
            _obstacleOriginPosition = Obstacle.transform.position;
            LifeController.Instance.OnPlayerDied+= ResetPosition;
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
        private void ResetPosition()
        {
            Obstacle.transform.position = _obstacleOriginPosition;
        }
    }
}
