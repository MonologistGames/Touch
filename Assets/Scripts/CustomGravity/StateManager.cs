using System;
using System.Collections.Generic;
using Tools.Singleton;
using UnityEngine;

namespace GlobalScripts
{
    public class StateManager : Singleton<StateManager>
    {
        private readonly List<object> _savedStates = new(); // ���һ�α����״̬
        private event Action<int> SaverManager; // ������������ڱ���浵ʱ��������״̬���溯��
        private event Action<int> ReaderManager; // ��ȡ���������ڶ�ȡ�浵ʱ��������״̬��ȡ����

        /// <summary>
        /// ע����Ҫ�浵��״̬
        /// </summary>
        /// <typeparam name="T">״̬����</typeparam>
        /// <param name="reader">״̬��ȡ����</param>
        /// <param name="saver">״̬���溯��</param>
        public void RegisterStateManager<T>(Func<T> reader, Action<T> saver)
        {
            _savedStates.Add(reader());
            ReaderManager += (idx) =>
            {
                saver((T)_savedStates[idx]);
            };
            SaverManager += (idx) =>
            {
                _savedStates[idx] = reader();
            };
        }

        /// <summary>
        /// ��ȡ��һ�α��������״̬
        /// </summary>
        public void ReadState()
        {
            for (int i = 0; i < _savedStates.Count; i++)
                ReaderManager?.Invoke(i);
        }

        /// <summary>
        /// ��������ע���״̬
        /// </summary>
        public void SaveState()
        {
            for (int i = 0; i < _savedStates.Count; i++)
                SaverManager?.Invoke(i);
        }
    }
}