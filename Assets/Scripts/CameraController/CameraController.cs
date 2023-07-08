using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Touch.CameraController
{
    public class CameraController : MonoBehaviour
    {
        public Transform PlayerPos;
        private BoxCollider _boxCollider;
        private CinemachineVirtualCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            _boxCollider=GetComponent<BoxCollider>();
            
            var distance=Mathf.Abs(transform.position.z-PlayerPos.position.z);
            var sizeY= 2*distance*Mathf.Tan(_camera.m_Lens.FieldOfView*Mathf.Deg2Rad/2);
            var a = Screen.width / Screen.height;
<<<<<<< Updated upstream
            _boxCollider.size=new Vector3(sizeY*a*0.75f,sizeY*0.75f,8f);
=======
            _boxCollider.size=new Vector3(sizeY*a,sizeY,2f);
>>>>>>> Stashed changes
            
            Debug.Log("distance:"+distance);
            Debug.Log("sizeY:"+sizeY);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Camera has changed to fixed camera");
            var cineBrain = other.GetComponent<CinemachineVirtualCamera>();
            cineBrain.Priority = 1;
        }
        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Camera has changed to free camera");
            var cineBrain = other.GetComponent<CinemachineVirtualCamera>();
            cineBrain.Priority = 11;
        }
    }
}

