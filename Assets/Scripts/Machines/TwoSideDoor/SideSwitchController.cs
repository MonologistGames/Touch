using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Touch.Machines.TwoSideDoor
{
    public class SideSwitchController : MonoBehaviour
    {
        public TwoSideDoorController MainScript;
        public GameObject ThePairDoor;
        public GameObject TheTriggerObject;
        public int DoorIndex;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject != TheTriggerObject) return;

            var temp = DoorIndex == 1 ? MainScript.Side1Target : MainScript.Side2Target;
            ThePairDoor.transform.DOLocalMove(
                temp,
                MainScript.DoorOpenSpeed
            );
            MainScript.IsActiveGravity = false;
        }
    }
}
