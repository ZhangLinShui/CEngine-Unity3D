using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEngine
{
    public class AssetBundlePath
    {
        public const string kWindows = "windows/";
        public const string kAndroid = "android/";
        public const string kIos = "ios/";

        public const string kAssetBundle = "AssetBundle";

        public string GetInternal()
        {
            return Application.streamingAssetsPath;
        }

        public string GetExternal()
        {
            return Application.persistentDataPath;
        }
    }
}
