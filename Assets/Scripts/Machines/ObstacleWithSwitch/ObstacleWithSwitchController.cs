using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Touch.PlayerLife;

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
        public string TriggerTag;
        public float EnterSpeed;
        public float ExitSpeed;
        
        private Vector3 _obstacleOriginPosition;
        private void Awake()
        {
            var tempCollider = Switch.GetComponent<BoxCollider>();
            tempCollider.size = SwitchAreaSize;
        }

        private void Start()
        {
            _obstacleOriginPosition = Obstacle.transform.position;
            LifeController.Instance.OnPlayerDied+= ResetPosition;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(147 / 255f, 213 / 255f, 220 / 255f, 0.5f);
            Gizmos.DrawCube(Switch.transform.position,SwitchAreaSize);
        }
        private void ResetPosition()
        {
            Obstacle.transform.position = _obstacleOriginPosition;
        }
    }
}

