using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Touch.CustomGravity;
using Touch.StateManager;

namespace Touch.PlayerController
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInputProcessor))]
    public class PlayerController : MonoBehaviour
    {
        #region ö��
        /// <summary>
        /// 玩家当前的状态
        /// </summary>
        public enum CharacterState
        {
            Floating, // 悬浮
            ChangeGravity, // 更改重力中
            Normal // 正常
        }

        public CharacterState State = CharacterState.Normal;
        private Animator _animator;

        [Header("Gravity Change")] [Tooltip("当更改状态后角色残留速度")]
        public float VelocityRemainAfterChange = 0.5f;
        [Tooltip("更改重力的 CD")]
        public float ChangeGravityColdTime = 0.5f;
        [Tooltip("最大速度")]
        [FormerlySerializedAs("VelocityLimited")]
        public float VelocityLimit = 6f;

        [Header("Gravity Change")]
        [Min(0f)]
        [Tooltip("重力系数的系数")]
        public float GravityFactorFactor = 1f;

        public float SlowTimeScale = 0.2f;

        [Header("Floating")]
        [Tooltip("悬浮时最大速度")]
        public float FloatingVelocityLimit = 1f;
        [Tooltip("悬浮的能量")]
        public float FloatingEnergy = 3f;
        [Tooltip("悬浮时能量回复速率")]
        public float FloatingEnergyRecoverRate = 2f;

        [Header("Color")]
        public Color GravityChangeColor;
        public Color ChangeReadyColor;
        public Color FloatingColor;

        [Header("Component")]
        public PlayerInputProcessor InputProcessor;
        #endregion

        #region 字段
        private float _currentChangeGravityColdTime; // 当前冷却时间
        private float _currentFloatingEnergy; // 当前悬浮能力
        private float _currentSlowTimeLapse;
        private bool _isExhausted; // 是否精疲力竭

        // 组件
        private Rigidbody _rigidbody;
        private Material _material;
        private Transform _transform;

        // 动画状态机
        private static readonly int Turn = Animator.StringToHash("Turn");
        private static readonly int Slow = Animator.StringToHash("Slow");
        #endregion

        #region 私有属性
        /// <summary>
        /// 当前的悬浮能量
        /// </summary>
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
        /// <summary>
        /// 是否能改变重力
        /// </summary>
        private bool CanChangeGravity => _currentChangeGravityColdTime <= 0f;
        /// <summary>
        /// 是否能悬浮
        /// </summary>
        private bool CanFloat => !_isExhausted && _currentFloatingEnergy > 0f;
        #endregion

        #region 共有属性
        /// <summary>
        /// 重生点
        /// </summary>
        public Vector3 Respawn { get; set; }
        #endregion

        #region 事件
        public event Action<float> OnFloatingEnergyChanged;
        #endregion

        #region 生命周期
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _transform = GetComponent<Transform>();
            CheckpointComponent.CheckpointEvent.AddListener((position) => { Respawn = position; }); // ��Ӽ���Ļص����� - ����������
        }

        private void Start()
        {
            _currentFloatingEnergy = FloatingEnergy;
            GlobalGravity.Instance.OnGravityChanged += ChangeGravityDirection;
            /* 注册所需保存的状态 */
            StateManager.StateManager.Instance.RegisterStateManager(() => Respawn, (value) => _transform.position = value); // 位置（根据重生点保存）
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
                        InputProcessor.IsFloating = false;
                        _animator.SetBool(Slow, false);
                        State = CharacterState.Normal;
                    }

                    if (!InputProcessor.IsFloating)
                    {
                        _animator.SetBool(Slow, false);
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
        #endregion

        #region Unity 事件
        /// <summary>
        /// 碰撞检测：当碰撞到default对象时死亡
        /// </summary>
        /// <remarks>
        /// 处理逻辑：
        /// <list type="bullet">
        /// <item><term>碰撞到<c>default</c>标签对象</term><description>角色死亡</description></item>
        /// </list>
        /// </remarks>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            // 死亡判定
            if (other.CompareTag("Default"))
                Die();
        }
        #endregion

        #region 私有方法
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
        public void SetRotationToVelocity()
        {
            transform.up = _rigidbody.velocity.normalized;
        }

        public void BeginChange()
        {
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

        private IEnumerator GravityChangeEffect()
        {
            BeginChange();
            yield return new WaitForSecondsRealtime(_currentSlowTimeLapse);
            EndChange();
        }
        #endregion

        /// <summary>
        /// 角色死亡逻辑
        /// </summary>
        private void Die()
        {
            // 读取存档点
            StateManager.StateManager.Instance.ReadState();
            // 重生方向和速度一样
            _rigidbody.velocity = GlobalGravity.Instance.GravityDirection.normalized;
            _transform.rotation = Quaternion.Euler(GlobalGravity.Instance.GravityDirection);
        }
        #endregion
    }
}