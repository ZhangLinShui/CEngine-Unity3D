using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEngine
{
    public class SceneTemplate<T> : MonoBehaviour where T : SceneTemplate<T>
    {
        public static T instance;

        protected MsgModel msg = new MsgModel();

        private void Awake()
        {
            instance = (T)this;

            OnInit();
        }

        protected virtual void OnInit()
        {
        }

        private void OnDestroy()
        {
            OnClear();

            msg.Clear();

            instance = null;
        }

        protected virtual void OnClear()
        {
        }
    }
}
