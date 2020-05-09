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

        public virtual void OnOpen(object[] args)
        {
        }

        public virtual void OnClear()
        {
        }

        public void CloseMySelf()
        {
            UIMgr.instance.ClosePeekUI(UIPath);
        }
    }
}
