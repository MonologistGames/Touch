using System;
using UnityEngine;
using UnityEngine.Serialization;
using Touch.CustomGravity;

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

        public CharacterState State = CharacterState.Normal;
        private Animator _animator;

        [Header("Gravity Change")] 
        public float VelocityRemainAfterChange = 0.5f;
        public float ChangeGravityColdTime = 0.5f;
        private float _currentChangeGravityColdTime;
        private bool CanChangeGravity => _currentChangeGravityColdTime <= 0f;


        [FormerlySerializedAs("VelocityLimited")] public float VelocityLimit = 6f;
        [Header("Gravity Change")] [Min(0f)] public float GravityFactorFactor = 1f;
        public float SlowTimeScale = 0.2f;
        
        [Header("Floating")]
        public float FloatingVelocityLimit = 1f;
        public float FloatingEnergy = 3f;
        public float FloatingEnergyRecoverRate = 2f;
        public event Action<float> OnFloatingEnergyChanged;

        private float CurrentFloatingEnergy
        {
            get => _currentFloatingEnergy;
            set
            {
                _currentFloatingEnergy = value;
                _currentFloatingEnergy = Mathf.Clamp(_currentFloatingEnergy, 0, FloatingEnergy);
                OnFloatingEnergyChanged?.Invoke(_currentFloatingEnergy);
            }
        }
        private float _currentFloatingEnergy;
        private bool _isExhausted;
        private bool CanFloat => !_isExhausted && _currentFloatingEnergy > 0f;
        
        private Rigidbody _rigidbody;

        public PlayerInputProcessor InputProcessor;
        private static readonly int Turn = Animator.StringToHash("Turn");

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _currentFloatingEnergy = FloatingEnergy;
            GlobalGravity.Instance.OnGravityChanged += ChangeGravityDirection;
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            #region Input

            if (InputProcessor.GravityDirection != Vector2.zero && CanChangeGravity)
            {
                if (GlobalGravity.Instance.ChangeDirection(InputProcessor.GravityDirection))
                    _currentChangeGravityColdTime = ChangeGravityColdTime;
            }

            #region Update Timer

            switch (State)
            {
                case CharacterState.Normal:
                    if (!CanChangeGravity)
                        _currentChangeGravityColdTime -= Time.unscaledDeltaTime;
                    if (InputProcessor.IsFloating && CanFloat)
                    {
                        State = CharacterState.Floating;
                        
                        // TODO: add floating effect
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
                        InputProcessor.IsFloating = false;
                        State = CharacterState.Normal;
                    }

                    if (!InputProcessor.IsFloating)
                    {
                        State = CharacterState.Normal;
                    }
                    break;
            }

            #endregion

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
                transform.up = _rigidbody.velocity.normalized;
        }


        private void ChangeGravityDirection(Vector3 gravity)
        {
            _animator.SetTrigger(Turn);
        }

        #region Gravity Change Effect

        public void BeginChange()
        {
            Debug.Log(_rigidbody.velocity);
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

        public void SetRotationToVelocity()
        {
            transform.up = _rigidbody.velocity.normalized;
        }

        #endregion
    }
}