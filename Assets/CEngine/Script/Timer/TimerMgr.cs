//********************************************************************
//	CopyRight   CEngine
//	Purpose:	定时器管理类
//	Created:	2020-04-28
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;
using System;

public class TimerMgr : SceneTemplate<TimerMgr>
{
    private LogicTimer _logicTimer = new LogicTimer();
    private FrameTimer _frameTimer = new FrameTimer();

    protected override void OnInit()
    {
        _logicTimer.OnAwake();
        _frameTimer.OnAwake(this);
    }

    private void Update()
    {
        _logicTimer.OnUpdate();
        _frameTimer.OnUpdate();
    }

    public static void SetLogicTimer(int delay, Action cb)
    {
        instance._logicTimer.SetTimer(delay, cb);
    }

    public static  void SetFrameTimer(int delay, Action cb)
    {
        instance._frameTimer.SetTimer(delay, cb);
    }
}
