using System;
using System.Collections.Generic;
using Tools.Singleton;
using UnityEngine;

namespace Touch.StateManager
{
    public class StateManager : Singleton<StateManager>
    {
        private readonly List<object> _savedStates = new(); // 最近一次保存的状态
        private List<Action<int>> SaverManager = new(); // 保存管理器，在保存存档时调用所有状态保存函数
        private List<Action<int>> ReaderManager = new(); // 读取管理器，在读取存档时调用所有状态读取函数

        /// <summary>
        /// 注册需要存档的状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="reader">状态读取函数</param>
        /// <param name="saver">状态保存函数</param>
        public void RegisterStateManager<T>(Func<T> reader, Action<T> saver)
        {
            _savedStates.Add(reader());
            ReaderManager.Add((idx) =>
            {
                saver((T)_savedStates[idx]);
            });
            SaverManager.Add((idx) =>
            {
                _savedStates[idx] = reader();
            });
        }

        /// <summary>
        /// 读取上一次保存的所有状态
        /// </summary>
        public void ReadState()
        {
            for (int i = 0; i < _savedStates.Count; i++)
                ReaderManager[i]?.Invoke(i);
        }

        /// <summary>
        /// 保存所有注册的状态
        /// </summary>
        public void SaveState()
        {
            for (int i = 0; i < _savedStates.Count; i++)
                SaverManager[i]?.Invoke(i);
        }
    }
}