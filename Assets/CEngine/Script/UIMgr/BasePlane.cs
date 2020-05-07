using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;

namespace CEngine
{
    public class BasePlane : MonoBehaviour
    {
        [HideInInspector]
        public string UIPath;

        protected MsgModel msg = new MsgModel();

        public virtual void OnOpen()
        {
        }

        public virtual void OnClear()
        {
            msg.Clear();
        }

        public void CloseMySelf()
        {
            UIMgr.instance.ClosePeekUI(UIPath);
        }
    }
}
