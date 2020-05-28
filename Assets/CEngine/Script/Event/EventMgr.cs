//********************************************************************
//	CopyRight   CEngine
//	Purpose:	事件管理类
//	Created:	2020-04-28
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件管理器[需主动注册释放]
/// </summary>
namespace CEngine
{   
    public class EventMgr : MgrTemplate<EventMgr>
    {
        public Dictionary<string, Delegate> _eventTable = new Dictionary<string, Delegate>();

        public void SendEvent(string msg)
        {
            Delegate d;
            if (_eventTable.TryGetValue(msg, out d))
            {
                var cb = (Action)d;
                if (null != d)
                {
                    cb();
                }
            }
        }

        public void SendEvent<T>(string msg, T arg1)
        {
            Delegate d;
            if (_eventTable.TryGetValue(msg, out d))
            {
                var cb = (Action<T>)d;
                if (null != d)
                {
                    cb(arg1);
                    GC.Collect(0);
                }
            }
        }

        public void SendEvent<T, U>(string msg, T arg1, U arg2)
        {
            Delegate d;
            if (_eventTable.TryGetValue(msg, out d))
            {
                var cb = (Action<T, U>)d;
                if (null != d)
                {
                    cb(arg1, arg2);
                    GC.Collect(0);
                }
            }
        }

        public void SendEvent<T, U, V>(string msg, T arg1, U arg2, V arg3)
        {
            Delegate d;
            if (_eventTable.TryGetValue(msg, out d))
            {
                var cb = (Action<T, U, V>)d;
                if (null != d)
                {
                    cb(arg1, arg2, arg3);
                    GC.Collect(0);
                }
            }
        }

        public void OnRegEvent(string msg)
        {
            if (!_eventTable.ContainsKey(msg))
            {
                _eventTable[msg] = null;
            }
        }

        public void RegEvent(string msg, Action cb)
        {
            OnRegEvent(msg);
            _eventTable[msg] = (Action)_eventTable[msg] + cb;
        }

        public void RegEvent<T>(string msg, Action<T> cb)
        {
            OnRegEvent(msg);
            _eventTable[msg] = (Action<T>)_eventTable[msg] + cb;
        }

        public void RegEvent<T, U>(string msg, Action<T, U> cb)
        {
            OnRegEvent(msg);
            _eventTable[msg] = (Action<T, U>)_eventTable[msg] + cb;
        }

        public void RegEvent<T, U, V>(string msg, Action<T, U, V> cb)
        {
            OnRegEvent(msg);
            _eventTable[msg] = (Action<T, U, V>)_eventTable[msg] + cb;
        }

        public void UnRegEvent(string msg, Action cb)
        {
            _eventTable[msg] = (Action)_eventTable[msg] - cb;
        }

        public void UnRegEvent<T>(string msg, Action<T> cb)
        {
            _eventTable[msg] = (Action<T>)_eventTable[msg] - cb;
        }

        public void UnRegEvent<T, U>(string msg, Action<T, U> cb)
        {
            _eventTable[msg] = (Action<T, U>)_eventTable[msg] - cb;
        }

        public void UnRegEvent<T, U, V>(string msg, Action<T, U, V> cb)
        {
            _eventTable[msg] = (Action<T, U, V>)_eventTable[msg] - cb;
        }
    }
}
