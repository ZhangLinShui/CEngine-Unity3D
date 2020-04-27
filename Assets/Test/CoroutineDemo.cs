using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineDemo
{
    private List<YieldItor> _lst;

    public void AddYieldItor(YieldItor yi)
    {
        _lst.Add(yi);
    }

    public void OnFrameStart()
    {
        for (int i = _lst.Count - 1; i >= 0; --i)
        {
            if (!_lst[i].MoveNext())
            {
                var t = _lst[i];
                _lst.RemoveAt(i);
                //执行
                t.CB();
                t.Parent.MoveNext();
            }
        }
    }
}

public class YieldItor : IEnumerator
{
    float t = Time.realtimeSinceStartup;

    public System.Action CB;

    public CoroutineItor Parent;

    public object Current
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public bool MoveNext()
    {
        if (Time.realtimeSinceStartup - t <= 3)
        {
            return true;
        }
        return false;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }
}

public class CoroutineItor : IEnumerator
{
    private object _current;
    private int _count;

    private CoroutineDemo _cd;

    public object Current
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public bool MoveNext()
    {
        if (_count < 2)
        {
            if (0 == _count)
            {
                _current = new YieldItor();

                var t = (YieldItor)_current;
                _cd.AddYieldItor(t);
            }
            else
            {
                _current = new YieldItor();
            }
            _count++;
            return true;
        }
        return false;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
    }
}
