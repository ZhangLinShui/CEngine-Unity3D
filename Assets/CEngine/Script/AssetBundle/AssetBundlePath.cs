using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEngine
{
    public class AssetBundlePath
    {
        public const string kWindows = "Windows/";
        public const string kAndroid = "Android/";
        public const string kIos = "Ios/";

        public const string kZipRes = "/res.zip";

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
