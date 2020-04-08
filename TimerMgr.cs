using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;
using System;

public class TimerMgr : MonoBehaviour
{
    private LogicTimer _logicTimer = new LogicTimer();
    private FrameTimer _frameTimer = new FrameTimer();

    private void Awake()
    {
        _logicTimer.OnAwake();
        _frameTimer.OnAwake(this);
    }

    private void Update()
    {
        _logicTimer.OnUpdate();
        _frameTimer.OnUpdate();
    }

    public void SetLogicTimer(int delay, Action cb)
    {
        _logicTimer.SetTimer(delay, cb);
    }

    public void SetFrameTimer(int delay, Action cb)
    {
        _frameTimer.SetTimer(delay, cb);
    }
}
