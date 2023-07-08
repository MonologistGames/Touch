using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Touch.Machines.ObstacleWithSwitch
{
    public class ObstacleWithSwitchController : MonoBehaviour
    {
        [Header("Pair")]
        public GameObject Switch;
        public GameObject Obstacle;
        [Header("Basic Settings")]
        public Vector3 SwitchAreaSize;
        public Vector3 ObstacleTargetPosition;
        public float EnterSpeed;
        public float ExitSpeed;
        
        private void Awake()
        {
            var tempCollider = Switch.GetComponent<BoxCollider>();
            tempCollider.size = SwitchAreaSize;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(147 / 255f, 213 / 255f, 220 / 255f, 0.5f);
            Gizmos.DrawCube(Switch.transform.position,SwitchAreaSize);
        }
    }
}

