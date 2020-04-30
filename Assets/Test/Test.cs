using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class JsonData
{
    public int A = 1;
    public int B = 2;

    public Dictionary<string, string> Dict = new Dictionary<string, string>() { { "key1", "value1" }, { "key2", "value2" } };
}

public class Test : MonoBehaviour
{
    public void Start()
    {
        var d1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        var d2 = new DateTime(1970, 1, 3, 18, 0, 0, DateTimeKind.Utc);

        Debug.LogError(d2.ToUniversalTime());

        var c = d1 - d2;
        //Debug.LogError(c.TotalMilliseconds);
    }
}
