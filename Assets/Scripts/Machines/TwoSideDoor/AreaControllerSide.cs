using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Touch.Machines.TwoSideDoor
{
    public class AreaControllerSide : MonoBehaviour
    {
        private TwoSideDoorController _twoSideDoorController;
        
        private void Awake()
        {
            _twoSideDoorController = GetComponentInParent<TwoSideDoorController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
        
            _twoSideDoorController.IsActiveGravity = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
        
            _twoSideDoorController.IsActiveGravity = false;
        }
    }
}
