using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Singleton;

namespace Touch.PlayerLife
{
    public class LifeController : Singleton<LifeController>
    {
        public float ReverseTime=1f;
        public PlayerController.PlayerController PlayerController;
        [Header("Debug")]
        public Vector3 ReversePosition;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            ReversePosition = transform.position;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag("Untagged"))return;
            
            Debug.Log("Player has died");
            StartCoroutine(ReversePositionCoroutine());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Flag")) return;

            ReversePosition = transform.position;
            
        }
        
        private IEnumerator ReversePositionCoroutine()
        {
            PlayerController.enabled= false;
            yield return new WaitForSeconds(ReverseTime);
            transform.position = ReversePosition;
            _rigidbody.velocity = Vector3.zero;
            PlayerController.enabled= true;
        }
    }
}
