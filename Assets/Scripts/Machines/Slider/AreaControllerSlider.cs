using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Touch.Machines.Slider
{
    public class AreaControllerSlider : MonoBehaviour
    {
        private SliderController _sliderController;

        private void Awake()
        {
            _sliderController = GetComponentInParent<SliderController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
        
            _sliderController.IsActiveGravity = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
        
            _sliderController.IsActiveGravity = false;
        }
    }
}

