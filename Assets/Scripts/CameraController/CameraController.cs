using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Touch.CameraController
{
    public class CameraController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var cineBrain = other.GetComponent<CinemachineVirtualCamera>();
            cineBrain.Priority = 11;
        }
        private void OnTriggerExit(Collider other)
        {
            var cineBrain = other.GetComponent<CinemachineVirtualCamera>();
            cineBrain.Priority = 1;
        }
    }
}

