using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEngine
{
    public class AssRef
    {
        public AssetBundle AB;
        public int Ref;
    }

    public class AssetBundleMgr : MgrTemplate<AssetBundleMgr>
    {
        private AssetBundleConfig _config;
        private Dictionary<string, AssRef> _dict = new Dictionary<string, AssRef>();

        public AssetBundle GetAssetBundle(string path)
        {
            AssRef assRef;
            if (!_dict.TryGetValue(path, out assRef))
            {
                _dict[path] = new AssRef();
            }
            assRef.Ref++;
            if (null == assRef.AB)
            {
                var abPath = _config.GetConfig(path);
                assRef.AB = AssetBundle.LoadFromFile(abPath);
            }
            return assRef.AB;
        }

        public void UnloadAssetBundle(string path)
        {
            AssRef assRef;
            if (_dict.TryGetValue(path, out assRef))
            {
                if (assRef.Ref > 0)
                {
                    assRef.Ref--;
                }
            }
            if (null != assRef && assRef.Ref == 0)
            {
                if (null != assRef.AB)
                {
                    assRef.AB.Unload(true);
                    assRef.AB = null;
                }
            }
        }
    }
}
