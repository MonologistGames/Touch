using System;
using Touch.CustomGravity;
using UnityEngine;

namespace Touch.PlayerController
{
    public class PlayerSoundEffect : MonoBehaviour
    {
        private PlayerController _playerController;
        public FMODUnity.StudioEventEmitter GravityChange;

        private void Start()
        {
            _playerController = GetComponentInParent<PlayerController>();
            GlobalGravity.Instance.OnGravityChanged += PlayGravityChange;
        }

        public void PlayGravityChange(Vector3 direction)
        {
            GravityChange.Play();
        }
    }
}