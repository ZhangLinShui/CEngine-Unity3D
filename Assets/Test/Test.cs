using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void Start()
    {
        var pos = gameObject.transform.position;
        pos.y += 100;
        transform.position = pos;

        var res = Resources.Load("GameObject");
        GameObject.Instantiate(res);
    }
}
