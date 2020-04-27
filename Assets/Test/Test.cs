using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YieldTest : CustomYieldInstruction
{
    private float st = Time.realtimeSinceStartup;

    public override bool keepWaiting
    {
        get
        {
            return Time.realtimeSinceStartup - st < 30f;
        }
    }
}

public class IEnumeratorTest : IEnumerator
{
    private float t = Time.realtimeSinceStartup;

    private Test _t;
    public IEnumeratorTest(Test t)
    {
        _t = t;
    }

    public object Current
    {
        get
        {
            Debug.LogError("get current" + _t._frame);
            return 0;
        }
    }

    public bool MoveNext()
    {
        Debug.LogError("move next " +  _t._frame);
        return true;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }
}

public class Test : MonoBehaviour
{
    public int _frame = 0;

    private void Update()
    {
        //Debug.LogError("update " + _frame++);
    }

    private void OnEnable()
    {
        //Debug.LogError("OnEnable");
    }

    IEnumerator _t;

    void Start()
    {
        //Debug.LogError("Start");

        //_t = RunTest();
        //StartCoroutine(new IEnumeratorTest(this));
        _t = RunTest();
        while (_t.MoveNext())
        {
            Debug.LogError("ok");
        }
    }

    private void FixedUpdate()
    {
        //Debug.LogError("fixed update");
    }

    IEnumerator RunTest()
    {
        Debug.LogError(_t.Current);

        yield return 100;

        Debug.LogError(_t.Current);

        yield return 101;

        Debug.LogError(_t.Current);

        yield return 102;

        Debug.LogError(_t.Current);
    }
}
