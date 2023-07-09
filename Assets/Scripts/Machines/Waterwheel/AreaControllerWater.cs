using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Touch.Machines.Waterwheel
{
    public class AreaControllerWater : MonoBehaviour
    {
        private WaterWheelController _waterWheelController;

        private void Awake()
        {
            _waterWheelController = GetComponentInParent<WaterWheelController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
        
            _waterWheelController.IsActiveGravity = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
        
            _waterWheelController.IsActiveGravity = false;
            _waterWheelController.ResetPosition(Vector3.zero);
        }
    }
}

