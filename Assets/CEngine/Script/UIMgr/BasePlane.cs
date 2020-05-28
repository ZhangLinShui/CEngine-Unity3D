//********************************************************************
//	CopyRight   CEngine
//	Purpose:	UI界面基类
//	Created:	2020-04-28
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
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
