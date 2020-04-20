using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;
using System;

public class TimerMgr : SceneTemplate<TimerMgr>
{
    private LogicTimer _logicTimer = new LogicTimer();
    private FrameTimer _frameTimer = new FrameTimer();

    protected override void OnAwake()
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

    public void SetFrameTimer(int delay, Action cb)
    {
        instance._frameTimer.SetTimer(delay, cb);
    }
}
