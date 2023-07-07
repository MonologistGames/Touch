using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalScripts;
using UnityEngine.Serialization;

namespace MainLogicalScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharaController : MonoBehaviour
    {
        public float velocityLimited = 6f;
        [Range(0,1)]
        public float gravityFactorFactor;
        private Rigidbody _rigidbody;
        private float _gravityFactor;
        private Vector3 _gravityDir;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _gravityDir = Vector3.down;
        }

        private void Start()
        {
            GlobalGravity.Instance.OnGravityChanged += ChangeGravityDirection;
            _gravityFactor = GlobalGravity.Instance.gravityFactor;
        }

        private void Update()
        {
            UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            var temp = _rigidbody.velocity;
            temp += gravityFactorFactor*_gravityFactor * Time.deltaTime * _gravityDir;
            temp = temp.normalized *
                   Mathf.Min(temp.magnitude, velocityLimited);
            _rigidbody.velocity = temp;
        }

        private void ChangeGravityDirection()
        {
            _gravityDir = GlobalGravity.Instance.GravityDirection;
        }
    }
}

