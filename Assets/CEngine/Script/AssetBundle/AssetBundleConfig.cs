using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CEngine
{
    [System.Serializable]
    public class AssetBundleFile
    {
        public bool IsInternal = true;
    }

    public class AssetBundleConfig : ScriptableObject
    {
        public AssetBundlePath _assetBundlePath;
        public Dictionary<string, AssetBundleFile> _dict = new Dictionary<string, AssetBundleFile>();

        public string GetConfig(string path)
        {
            //todo: 修复
            return "";
        }
    }
}
