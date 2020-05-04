using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using CEngine;
using System.IO;

public class Test : MonoBehaviour
{
    public void Start()
    {
        var ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + AssetBundlePath.kSlash + "common.unity3d");
        var res = ab.LoadAsset<GameObject>("test1");

        GameObject.Instantiate<GameObject>(res);
    }
}
