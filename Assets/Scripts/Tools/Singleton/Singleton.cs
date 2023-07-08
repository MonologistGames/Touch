using System;
using UnityEngine;


namespace Tools.Singleton
{
    /// <summary>
    /// 单例模式工具
    /// </summary>
    /// <remarks>
    /// 使用方法：让需要单例的类继承该类
    /// </remarks>
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;
        public static T Instance => _instance ??= new T();

        /*
        protected virtual void Awake()
        {

            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(this.gameObject);
        }
        */
    }
}
