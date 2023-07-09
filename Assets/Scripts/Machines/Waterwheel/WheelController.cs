using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Touch.CustomGravity;

namespace Touch.Machines.Waterwheel
{
    public class WheelController : MonoBehaviour
    {
        public GameObject Door;
        public float DoorOpenSpeed=1f;
        public Vector3 DoorTargetPosition;
        public float RotateTime=1f;
        public float RotateSpeed;
        private bool _isAllowRotate;
        private float _rotateTime=0;

        private void Start()
        {
            GlobalGravity.Instance.OnGravityChanged += ResetRotate;
        }

        private void FixedUpdate()
        {
            if (_isAllowRotate)
            {
                transform.Rotate(
                    Vector3.back,
                    RotateSpeed*Time.deltaTime,
                    Space.Self);
                _rotateTime+= Time.deltaTime;
            }
            else
            {
                _rotateTime = 0;
            }

            if (_rotateTime >= RotateTime)
            {
                Door.transform.DOMove(DoorTargetPosition, DoorOpenSpeed);
                Destroy(this);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag("Water")) return;
            
            if(GlobalGravity.Instance.Gravity.y<0)
                _isAllowRotate = true;
            else
            {
                _isAllowRotate = false;
            }
        }
        private void ResetRotate(Vector3 gravity)
        {
            _isAllowRotate = false;
        }
    }
}
