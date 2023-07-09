using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Singleton;
using Touch.CustomGravity;

namespace Touch.PlayerLife
{
    public class LifeController : Singleton<LifeController>
    {
        private bool _isDead = false;
        public float ReverseTime=1f;
        public PlayerController.PlayerController PlayerController;
        [Header("Debug")]
        public Vector3 ReversePosition;

        public Vector3 ReverseGravity;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            ReversePosition = transform.position;
            ReverseGravity = GlobalGravity.Instance.GravityDirection;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag("Untagged"))return;
            
            Debug.Log("Player has died");
            if (_isDead) return;
            _isDead = true;
            StartCoroutine(ReversePositionCoroutine());
        }
        
        // Entering respawn point
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Flag")) return;
            ReversePosition = other.transform.position;
            ReverseGravity = GlobalGravity.Instance.GravityDirection;
        }
        
        private IEnumerator ReversePositionCoroutine()
        {
            PlayerController.enabled= false;
            PlayerController.DieEffect();
            _rigidbody.velocity = Vector3.zero;
            yield return new WaitForSeconds(ReverseTime);
            transform.position = ReversePosition;
            PlayerController.enabled= true;
            GlobalGravity.Instance.ResetGDirection(ReverseGravity);
            _isDead = false;
        }
    }
}
