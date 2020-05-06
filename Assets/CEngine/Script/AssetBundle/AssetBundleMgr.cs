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
        public string[] CompressExts = new string[] { ".unity3d", ".bytes", ".cfg" };
        public string kPatchFileExt = ".bytes";

        /* Most platforms (Unity Editor, Windows, Linux players, PS4, Xbox One, Switch) use Application.dataPath + "/StreamingAssets",
         * macOS player uses Application.dataPath + "/Resources/Data/StreamingAssets",
         * iOS uses Application.dataPath + "/Raw",
         * Android uses files inside a compressed APK JAR file, "jar:file://" + Application.dataPath + "!/assets".*/
        private string _streamingAssetPath;
        public string StreamingAssetPath
        {
            get
            {
                if (string.IsNullOrEmpty(_streamingAssetPath))
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        _streamingAssetPath = Application.streamingAssetsPath + "/";
                    }
                    else
                    {
                        _streamingAssetPath = "file://" + Application.streamingAssetsPath + "/";
                    }
                }
                return _streamingAssetPath;
            }
        }

        /// <summary>
        /// 压缩包路径
        /// </summary>
        private string _zipRes;
        public string ZipRes
        {
            get
            {
                if(string.IsNullOrEmpty(_zipRes))
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        _zipRes = AssetBundlePath.kAndroidZipRes;
                    }
                    else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        _zipRes = AssetBundlePath.kIosZipRes;
                    }
                    else
                    {
                        _zipRes = AssetBundlePath.kWindowsZipRes;
                    }
                }
                return _zipRes;
            }
        }

        /// <summary>
        /// 压缩包解压后文件夹路径
        /// </summary>
        private string _zipFolder;
        public string ZipFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_zipFolder))
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        _zipFolder = AssetBundlePath.kAndroidZipFolder;
                    }
                    else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        _zipFolder = AssetBundlePath.kIosZipFolder;
                    }
                    else
                    {
                        _zipFolder = AssetBundlePath.kWindowsZipFolder;
                    }
                }
                return _zipFolder;
            }
        }

        /// <summary>
        /// 补丁压缩包路径
        /// </summary>
        private string _patchZip;
        public string PatchZip
        {
            get
            {
                if (string.IsNullOrEmpty(_patchZip))
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        _patchZip = AssetBundlePath.kAndroid + AssetBundlePath.kPatchZipRes;
                    }
                    else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        _patchZip = AssetBundlePath.kIos + AssetBundlePath.kPatchZipRes;
                    }
                    else
                    {
                        _patchZip = AssetBundlePath.kWindows + AssetBundlePath.kPatchZipRes;
                    }
                }
                return _patchZip;
            }
        }

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
                //var abPath = _config.GetConfig(path);
                //todo: 修复
                assRef.AB = AssetBundle.LoadFromFile("");
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
