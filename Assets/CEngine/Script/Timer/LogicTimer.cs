//********************************************************************
//	CopyRight   CEngine
//	Purpose:	逻辑定时器
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
    /// 逻辑延迟定时器
    /// </summary>
    public class LogicTimer : BaseTimer
    {
        private Stopwatch _sw = new Stopwatch();
        private long _lastMilliSecond = 0;
        private int kTickInterval = 1;

        public void OnAwake()
        {
            _sw.Start();
        }

        public void OnUpdate()
        {
            var t = _sw.ElapsedMilliseconds - _lastMilliSecond;
            while (t - kTickInterval >= 0)
            {
                t -= kTickInterval;
                _lastMilliSecond += kTickInterval;

                Wheel.AddTickCounter();
                Wheel.Tick();
            }
        }
    }
}
