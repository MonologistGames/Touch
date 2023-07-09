using System;
using Touch.CustomGravity;
using UnityEngine;

namespace Touch.PlayerController
{
    public class PlayerSoundEffect : MonoBehaviour
    {
        public FMODUnity.StudioEventEmitter GravityChange;

        private void Start()
        {
            GlobalGravity.Instance.OnGravityChanged += PlayGravityChange;
        }

        public void PlayGravityChange(Vector3 direction)
        {
            GravityChange.Play();
        }
    }
}