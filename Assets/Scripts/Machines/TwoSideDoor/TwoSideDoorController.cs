using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Touch.Machines.TheObjectController;

namespace Touch.Machines.TwoSideDoor
{
    public class TwoSideDoorController : ObjectController
    {
        [Header("Door Settings")] 
        public Vector3 Side1Target;
        public Vector3 Side2Target;
        public float DoorOpenSpeed;
    }
}

