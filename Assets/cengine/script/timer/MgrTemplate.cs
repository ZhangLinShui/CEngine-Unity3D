using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MgrTemplate<T> where T: MgrTemplate<T>, new()
{
    private static T _inst;

    public static T instance
    {
        get
        {
            if (null == _inst)
            {
                _inst = new T();
            }
            return _inst;
        }
    }

    public virtual void OnInit()
    {
    }

    public virtual void OnDispose()
    {
        _inst = null;
    }
}
