using System;
using UnityEngine;
using Tools.Singleton;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Touch.CustomGravity
{
    /// <summary>
    /// ȫ��״̬���� - ����ϵͳ
    /// </summary>
    public class GlobalGravity : Singleton<GlobalGravity>
    {
        #region Unity ����
        [Tooltip("ȫ������ϵ��")]
        public float GravityFactor = 9.8f;
        #endregion

        #region �¼�
        /// <summary>
        /// ���������򣩸���ʱ
        /// </summary>
        /// <remarks>
        /// �ڽ�ɫ�������н���ע��
        /// </remarks>
        public event Action<Vector3> OnGravityChanged;
        #endregion

        #region ˽���ֶ�
        private Vector3 _gravityDir = Vector3.down; // ��ǰ��������
        #endregion

        #region ����
        /// <summary>
        /// �����������
        /// </summary>
        public Vector3 GravityDirection => _gravityDir;
        /// <summary>
        /// ���������ϵ���˷���
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