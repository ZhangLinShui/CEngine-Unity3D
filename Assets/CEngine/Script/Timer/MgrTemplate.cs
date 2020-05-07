using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEngine
{
    public class MgrTemplate<T> where T : MgrTemplate<T>, new()
    {
        private static T _inst;

        protected MsgModel msg = new MsgModel();

        public static T instance
        {
            get
            {
                if (null == _inst)
                {
                    _inst = new T();
                }
                return _inst;
            }
        }

        public void Init()
        {
            OnInit();
        }

        protected virtual void OnInit()
        {
        }

        public void Dispose()
        {
            OnClear();

            msg.Clear();

            _inst = null;
        }

        protected virtual void OnClear()
        {
        }
    }

}