using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using CEngine;

public class Test : MonoBehaviour
{
    public void Start()
    {
        var d = new PackageCfg();
        d.Files.Add(new FileCfg("chen", "tao"));

        Debug.LogError(JsonUtility.ToJson(d));
    }
}
