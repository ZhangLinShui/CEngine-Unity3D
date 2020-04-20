using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEngine
{
    public class SceneTemplate<T> : MonoBehaviour where T : SceneTemplate<T>
    {
        public static T instance;

        private void Awake()
        {
            instance = (T)this;

            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }

        private void OnDestroy()
        {
            OnClear();

            instance = null;
        }

        protected virtual void OnClear()
        {
        }
    }
}
