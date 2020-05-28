//********************************************************************
//	CopyRight   CEngine
//	Purpose:	主Canvas类
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
    public class MainCanvas : SceneTemplate<MainCanvas>
    {
        protected override void OnInit()
        {
            GameObject.DontDestroyOnLoad(this);
        }
    }
}
