using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;
using System;

namespace CEngine
{
    public enum EWinType
    {
        Temp,
        Normal,
    }

    public class UIMgr : SceneTemplate<UIMgr>
    {
        private List<PlaneData> _windows = new List<PlaneData>();

        public GameObject RootPlane;

        public class PlaneData
        {
            public object Params;
            public string AssetBundlekey;
            public EWinType WinType;
            public BasePlane bp;
        }

        protected override void OnAwake()
        {
        }

        public void OpenUI(string path, EWinType winType, object o)
        {
            if (_windows.Count > 0)
            {
                var d = _windows[_windows.Count - 1];
                if (path == d.AssetBundlekey)
                {
                    return;
                }
                if (d.WinType == EWinType.Temp)
                {
                    d.bp.OnClear();
                    AssetBundleMgr.instance.UnloadAssetBundle(d.bp.AssetBundleKey);
                    GameObject.Destroy(d.bp);
                    _windows.RemoveAt(_windows.Count - 1);
                    d.bp = null;
                }
                else if (d.WinType == EWinType.Normal)
                {
                    d.bp.OnClear();
                    AssetBundleMgr.instance.UnloadAssetBundle(d.bp.AssetBundleKey);
                    GameObject.Destroy(d.bp);
                    d.bp = null;
                }
            }
            var pd = new PlaneData();
            pd.AssetBundlekey = path;
            pd.WinType = winType;
            pd.Params = o;
            pd.bp = AssetBundleMgr.instance.GetAssetBundle(path).LoadAsset<GameObject>(path).GetComponent<BasePlane>();
            pd.bp.AssetBundleKey = path;
            pd.bp.transform.parent = RootPlane.transform;

            pd.bp.OnOpen();

            _windows.Add(pd);

            GC.Collect();
        }

        public void CloseUI(string abkey)
        {
            var index = _windows.Count - 1;
            if (_windows[index].AssetBundlekey != abkey)
            {
                return;
            }
            var pd = _windows[_windows.Count - 1];
            AssetBundleMgr.instance.UnloadAssetBundle(_windows[_windows.Count - 1].bp.AssetBundleKey);
            if (null != pd.bp)
            {
                pd.bp.OnClear();
                GameObject.Destroy(pd.bp);
            }
            _windows.RemoveAt(index);

            if (pd.WinType == EWinType.Normal)
            {
                var prevPd = _windows[_windows.Count - 1];
                prevPd.bp = AssetBundleMgr.instance.GetAssetBundle(prevPd.AssetBundlekey).LoadAsset<GameObject>(prevPd.AssetBundlekey).GetComponent<BasePlane>();
                prevPd.bp.AssetBundleKey = prevPd.AssetBundlekey;
                prevPd.bp.transform.parent = RootPlane.transform;
                prevPd.bp.OnOpen();
            }
            GC.Collect();
        }
    }
}
