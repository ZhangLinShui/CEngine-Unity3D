//********************************************************************
//	CopyRight   CEngine
//	Purpose:	更新界面类
//	Created:	2020-04-28
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class LoginPlane : BasePlane
    {
        public Slider _slider;

        public override void OnOpen(object[] args)
        {
            EventMgr.instance.RegEvent<float>(EventDefine.UpdateProgress, OnUpdateProgress);
        }

        public override void OnClear()
        {
            EventMgr.instance.UnRegEvent<float>(EventDefine.UpdateProgress, OnUpdateProgress);
        }

        private void OnUpdateProgress(float progress)
        {
            _slider.value = progress;
        }
    }
}
