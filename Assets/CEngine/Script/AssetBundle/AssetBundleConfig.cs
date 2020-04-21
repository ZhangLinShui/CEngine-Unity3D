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
#if UNITY_EDITOR
            return Application.streamingAssetsPath + AssetBundlePath.kWindows + path;
#elif UNITY_ANDROID
            if (_dict[path].IsInternal)
            {
                return _assetBundlePath.GetInternal() + AssetBundlePath.kAndroid + path;
            }
            return _assetBundlePath.GetExternal() + AssetBundlePath.kAndroid + path;
#elif UNITY_IOS
            if (_dict[path].IsInternal)
            {
                return _assetBundlePath.GetInternal() + AssetBundlePath.kIos + path;
            }
            return _assetBundlePath.GetExternal() + AssetBundlePath.kIos + path;
#endif
            return "";
        }
    }
}
