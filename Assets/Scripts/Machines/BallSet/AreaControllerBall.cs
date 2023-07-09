using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Touch.Machines.BallSet
{
    public class AreaControllerBall : MonoBehaviour
    {
        private BallSetController _ballSetController;

        private void Awake()
        {
            _ballSetController = GetComponentInParent<BallSetController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
        
            _ballSetController.IsActiveGravity = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
        
            _ballSetController.IsActiveGravity = false;
        }
    }
}
