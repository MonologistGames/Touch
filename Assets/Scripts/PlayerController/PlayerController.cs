using System;
using UnityEngine;
using Touch.CustomGravity;
using Unity.Mathematics;

namespace Touch.PlayerController
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInputProcessor))]
    public class PlayerController : MonoBehaviour
    {
        public enum CharacterState
        {
            Floating,
            ChangeGravity,
            Normal
        }

     
        private Animator _animator;
        private PlayerInputProcessor _inputProcessor;
        private Rigidbody _rigidbody;
        
        public float GravityFactorFactor = 1f;
        public CharacterState State = CharacterState.Normal;

        #region Gravity Change

        [Header("Gravity Change")]
        public float VelocityRemainAfterChange = 0.5f;
        public float ChangeGravityColdTime = 0.5f;
        private float _currentChangeGravityColdTime;
        public float VelocityLimit = 6f;
        [Min(0f)] public float SlowTimeScale = 0.2f;
        
        private bool CanChangeGravity => _currentChangeGravityColdTime <= 0f;
        
        public float CurrentGChangeCd => _currentChangeGravityColdTime;

        #endregion

        #region Floating

        [Header("Floating")] public float FloatingVelocityLimit = 1f;
        public float FloatingEnergy = 3f;
        public float FloatingEnergyRecoverRate = 2f;
        private float _currentFloatingEnergy;
        private bool _isExhausted;
        
        private bool CanFloat => !_isExhausted && _currentFloatingEnergy > 0f;
        
        private float CurrentFloatingEnergy
        {
            get => _currentFloatingEnergy;
            set
            {
                var v = Mathf.Clamp(value, 0, FloatingEnergy);
                if (Mathf.Approximately(v, _currentFloatingEnergy)) 
                    return;
                OnFloatingEnergyChanged?.Invoke(_currentFloatingEnergy, _isExhausted);
                _currentFloatingEnergy = v;
            }
        }
        
        #endregion
        
        #region Animation Params

        private static readonly int Turn = Animator.StringToHash("Turn");
        private static readonly int Slow = Animator.StringToHash("Slow");

        #endregion

        public event Action<float, bool> OnFloatingEnergyChanged;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _inputProcessor = GetComponent<PlayerInputProcessor>();
            
            // Initialize
            _currentFloatingEnergy = FloatingEnergy;
            // Register Events
            GlobalGravity.Instance.OnGravityChanged += ChangeGravityDirection;
        }

        private void Update()
        {
            #region Input

            if (_inputProcessor.GravityDirection != Vector2.zero && CanChangeGravity)
            {
                GlobalGravity.Instance.ChangeDirection(_inputProcessor.GravityDirection);
            }

            #endregion

            #region Update Timer

            switch (State)
            {
                case CharacterState.Normal:
                    if (!CanChangeGravity)
                        _currentChangeGravityColdTime -= Time.deltaTime;
                    if (_inputProcessor.IsFloating && CanFloat)
                    {
                        State = CharacterState.Floating;
                        _animator.SetBool(Slow, true);
                    }
                    else
                    {
                        CurrentFloatingEnergy += Time.deltaTime * FloatingEnergyRecoverRate;
                        if (Mathf.Approximately(CurrentFloatingEnergy, FloatingEnergy))
                            _isExhausted = false;
                    }

                    break;

                case CharacterState.Floating:
                    CurrentFloatingEnergy -= Time.deltaTime;
                    if (CurrentFloatingEnergy <= 0f)
                    {
                        _isExhausted = true;
                        _inputProcessor.IsFloating = false;
                        _animator.SetBool(Slow, false);
                        State = CharacterState.Normal;
                    }

                    if (!_inputProcessor.IsFloating)
                    {
                        _animator.SetBool(Slow, false);
                        State = CharacterState.Normal;
                    }

                    break;
            }

            #endregion
        }

        private void FixedUpdate()
        {
            #region Update

            switch (State)
            {
                case CharacterState.Normal:
                    UpdateVelocity(VelocityLimit);
                    UpdateRotation();
                    break;
                case CharacterState.Floating:
                    UpdateVelocity(FloatingVelocityLimit);
                    UpdateRotation();
                    break;
                case CharacterState.ChangeGravity:
                    UpdateVelocity(VelocityLimit);
                    break;
            }

            #endregion
        }

        private void UpdateVelocity(float velocityLimit)
        {
            var temp = _rigidbody.velocity;
            temp += GlobalGravity.Instance.Gravity * (GravityFactorFactor * Time.fixedUnscaledDeltaTime);
            temp = temp.normalized *
                   Mathf.Min(temp.magnitude, velocityLimit);
            _rigidbody.velocity = temp;
        }

        private void UpdateRotation()
        {
            if (_rigidbody.velocity.sqrMagnitude > 0.5f)
                SetRotationToVelocity();
        }


        private void ChangeGravityDirection(Vector3 gravity)
        {
            _animator.SetTrigger(Turn);
        }

        #region Gravity Change Effect

        public void SetRotationToVelocity()
        {
            var dotResult = Vector3.Dot(_rigidbody.velocity.normalized,Vector3.right);
            var angle = Mathf.Acos(dotResult);
            if (_rigidbody.velocity.y < 0) angle = -angle;
            transform.rotation =
                Quaternion.AngleAxis(angle * Mathf.Rad2Deg - 90f, Vector3.forward);
        }

        public void BeginChange()
        {
            _currentChangeGravityColdTime = ChangeGravityColdTime;
            _rigidbody.velocity = _rigidbody.velocity.normalized * VelocityRemainAfterChange;
            Time.timeScale = SlowTimeScale;
            Time.fixedDeltaTime *= SlowTimeScale;
            State = CharacterState.ChangeGravity;
        }

        public void EndChange()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            State = CharacterState.Normal;
        }

        #endregion
    }
}