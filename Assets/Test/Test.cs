using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using CEngine;
using System.IO;
using CEngine;

namespace GameLogic
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            var mirror = AssetBundleMgr.instance.GetAssetBundle("ui/common.unity3d").LoadAsset<GameObject>("GoodMan");
            GameObject.Instantiate(mirror);
        }

        private void Update()
        {
        }
    }
}