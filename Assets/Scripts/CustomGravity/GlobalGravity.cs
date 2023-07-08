using System;
using UnityEngine;
using Tools.Singleton;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Touch.CustomGravity
{
    /// <summary>
    /// 全局状态管理 - 重力系统
    /// </summary>
    public class GlobalGravity : Singleton<GlobalGravity>
    {
        #region Unity 参数
        [Tooltip("全局重力系数")]
        public float GravityFactor = 9.8f;
        #endregion

        #region 事件
        /// <summary>
        /// 重力（方向）更改时
        /// </summary>
        /// <remarks>
        /// 在角色控制器中进行注册
        /// </remarks>
        public event Action<Vector3> OnGravityChanged;
        #endregion

        #region 私有字段
        private Vector3 _gravityDir = Vector3.down; // 当前重力方向
        #endregion

        #region 属性
        /// <summary>
        /// 获得重力方向
        /// </summary>
        public Vector3 GravityDirection => _gravityDir;
        /// <summary>
        /// 获得重力（系数乘方向）
        /// </summary>
        public Vector3 Gravity => GravityFactor * _gravityDir;
        #endregion
        public bool ChangeDirection(Vector3 direction)
        {
            if (_gravityDir == direction) return false;
            
            _gravityDir = direction;
            OnGravityChanged?.Invoke(Gravity);
            return true;
        }
    }
}