using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件监听器
/// </summary>
namespace CEngine
{
    public class MsgModel
    {
        private Dictionary<string, Action> _dict = new Dictionary<string, Action>();

        public void RegEvent(string msg, Action cb)
        {
            if (_dict.ContainsKey(msg))
            {
                TimeLogger.LogError("msg repeated register " + msg);
                return;
            }
            _dict[msg] = cb;

            EventListener.instance.RegEvent(msg, cb);
        }

        public void Clear()
        {
            foreach (var kvp in _dict)
            {
                EventListener.instance.UnRegEvent(kvp.Key, kvp.Value);
            }
        }
    }

    public class EventListener : MgrTemplate<EventListener>
    {
        private Dictionary<string, List<Action>> _dict = new Dictionary<string, List<Action>>();

        public void SendEvent(string msg)
        {
            List<Action> callbacks;
            if (_dict.TryGetValue(msg, out callbacks))
            {
                foreach (var cb in callbacks)
                {
                    if (null != cb)
                    {
                        cb();
                    }
                }
            }
        }

        public void RegEvent(string msg, Action cb)
        {
            if (!_dict.ContainsKey(msg))
            {
                _dict[msg] = new List<Action>();
            }
            _dict[msg].Add(cb);
        }

        public void UnRegEvent(string msg, Action cb)
        {
            List<Action> l;
            if (_dict.TryGetValue(msg, out l))
            {
                Action tmp;
                for (int i = l.Count - 1; i >= 0; --i)
                {
                    tmp = l[i];
                    if (tmp == cb)
                    {
                        l.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
