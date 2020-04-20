using System.Collections;
using System.Collections.Generic;
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
            if (_dict[path].IsInternal)
            {
                return _assetBundlePath.GetInternal() + "/" + path;
            }
            return _assetBundlePath.GetExternal() + "/" + path;
        }
    }
}
