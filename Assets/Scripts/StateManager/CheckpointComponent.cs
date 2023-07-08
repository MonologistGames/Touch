using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Touch.StateManager
{
    public class CheckpointComponent : MonoBehaviour
    {
        private void OnTriggerEnter(Collider obj)
        {
            if (obj.CompareTag("Player"))
                StateManager.Instance.SaveState();
        }
    }
}