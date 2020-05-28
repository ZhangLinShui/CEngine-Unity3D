//********************************************************************
//	CopyRight   CEngine
//	Purpose:	帧定时器
//	Created:	2020-04-28
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace CEngine
{
    /// <summary>
    /// 帧定时器
    /// </summary>
    public class FrameTimer : BaseTimer
    {
        public void OnAwake(MonoBehaviour mb)
        {
            mb.StartCoroutine(RunFrameTimer());
        }

        IEnumerator RunFrameTimer()
        {
            while (true)
            {
                yield return 0;

                Wheel.AddTickCounter();
            }
        }

        public void OnUpdate()
        {
            Wheel.Tick();
        } 
    }
}
