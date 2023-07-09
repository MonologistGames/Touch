using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Touch.Machines.ObstacleWithSwitch
{
    public class SwitchController : MonoBehaviour
    {
        private ObstacleWithSwitchController _obstacleWithSwitchController;
        private Transform _obstacleTransform;
        private Vector3 _obstacleOriginalPos;
        private void Awake()
        {
            _obstacleWithSwitchController = 
                GetComponentInParent<ObstacleWithSwitchController>();
            _obstacleTransform = _obstacleWithSwitchController.Obstacle.transform;
            _obstacleOriginalPos = _obstacleTransform.localPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_obstacleWithSwitchController.TriggerTag)) return;
            
            _obstacleTransform.DOLocalMove(
                _obstacleWithSwitchController.ObstacleTargetPosition, 
                _obstacleWithSwitchController.EnterSpeed);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(_obstacleWithSwitchController.TriggerTag)) return;
            
            _obstacleTransform.DOLocalMove(
                _obstacleOriginalPos, 
                _obstacleWithSwitchController.ExitSpeed);
        }
    }
}
