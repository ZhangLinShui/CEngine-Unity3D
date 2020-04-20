using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEngine
{
    public class AssetBundlePath
    {
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
