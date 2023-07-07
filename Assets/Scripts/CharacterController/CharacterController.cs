using System.Collections;
using System.Diagnostics;
using UnityEngine;
using GlobalScripts;

namespace MainLogicalScripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInputProcessor))]
    public class CharacterController : MonoBehaviour
    {
        public enum CharacterState
        {
            Floating,
            ChangeGravity,
            Normal
        }

        public CharacterState State = CharacterState.Normal;

        [Header("Gravity Change")] public float VelocityRemainAfterChange = 0.5f;
        public float ChangeGravityColdTime = 0.5f;
        private float _currentChangeGravityColdTime;
        private bool CanChangeGravity => _currentChangeGravityColdTime <= 0f;


        public float VelocityLimited = 6f;
        [Header("Gravity Change")] [Min(0f)] public float GravityFactorFactor = 1f;

        public float SlowTimeLapse = 0.2f;
        private float _currentSlowTimeLapse;
        public float SlowTimeScale = 0.2f;

        private Rigidbody _rigidbody;

        private Material _material;

        public Color GravityChangeColor;
        public Color ChangeReadyColor;

        public PlayerInputProcessor InputProcessor;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            GlobalGravity.Instance.OnGravityChanged += ChangeGravityDirection;
            _material = GetComponent<MeshRenderer>().material;
        }

        private void FixedUpdate()
        {
            #region Input

            if (InputProcessor.GravityDirection != Vector2.zero && CanChangeGravity)
            {
                if (GlobalGravity.Instance.ChangeDirection(InputProcessor.GravityDirection))
                    _currentChangeGravityColdTime = ChangeGravityColdTime;
            }

            #endregion

            #region Update Timer

            if (!CanChangeGravity)
                _currentChangeGravityColdTime -= Time.unscaledDeltaTime;

            #endregion

            #region Update

            switch (State)
            {
                case CharacterState.Normal:
                    UpdateRotation();
                    break;
            }

            UpdateVelocity();

            if (CanChangeGravity) _material.color = ChangeReadyColor;

            #endregion
        }

        private void UpdateVelocity()
        {
            var temp = _rigidbody.velocity;
            temp += GlobalGravity.Instance.Gravity * (GravityFactorFactor * Time.fixedDeltaTime);
            temp = temp.normalized *
                   Mathf.Min(temp.magnitude, VelocityLimited);
            _rigidbody.velocity = temp;
        }

        private void UpdateRotation()
        {
            if (_rigidbody.velocity.sqrMagnitude > 0.5f)
                transform.up = _rigidbody.velocity.normalized;
        }


        private void ChangeGravityDirection(Vector3 gravity)
        {
            StartCoroutine(GravityChangeEffect());
        }

        #region Gravity Change Effect

        public void BeginChange()
        {
            _material.color = GravityChangeColor;
            _rigidbody.velocity *= VelocityRemainAfterChange;
            
            Time.timeScale = SlowTimeScale;
            Time.fixedDeltaTime *= SlowTimeScale;
            
            State = CharacterState.ChangeGravity;
        }

        public void EndChange()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;

            State = CharacterState.Normal;

            _material.color = Color.white;
        }

        public void SetRotationToVelocity()
        {
            transform.up = _rigidbody.velocity.normalized;
        }

        #endregion

        private IEnumerator GravityChangeEffect()
        {
            BeginChange();
            yield return new WaitForSecondsRealtime(SlowTimeLapse);
            EndChange();
        }
    }
}