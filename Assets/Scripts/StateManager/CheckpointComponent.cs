using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Touch.StateManager
{
    /// <summary>
    /// 检查点 组件
    /// </summary>
    public class CheckpointComponent : MonoBehaviour
    {
        public static UnityEvent<Vector3> CheckpointEvent = new();

        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        #region 生命周期
        // TODO: 检查逻辑：当触发存档点碰撞箱的触发器后，进行存档
        private void OnTriggerEnter(Collider obj)
        {
            CheckpointEvent.Invoke(_transform.position);
            if (obj.CompareTag("Player"))
                StateManager.Instance.SaveState();
        }
        #endregion
    }
}