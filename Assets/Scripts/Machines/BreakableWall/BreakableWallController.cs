using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Touch.Machines.BreakableWallController
{
    public class BreakableWallController : MonoBehaviour
    {
        public string HitObjectTag;
        public float HitMinVelocity=4f;
        [Header("Debug")] public int HitCount;

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag(HitObjectTag)) return;
            
            if(other.relativeVelocity.magnitude>=HitMinVelocity&&++HitCount>=3)
                Destroy(gameObject);
        }
    }
}
