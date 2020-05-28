//********************************************************************
//	CopyRight   CEngine
//	Purpose:	场景模板类
//	Created:	2020-04-28
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
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

            OnInit();
        }

        protected virtual void OnInit()
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
