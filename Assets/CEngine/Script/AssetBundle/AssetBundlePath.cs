using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEngine
{
    public class AssetBundlePath
    {
        public const string kWindows = "Windows";
        public const string kAndroid = "Android";
        public const string kIos = "Ios";

        public const string kAndroidZipFolder = "androidRes";
        public const string kAndroidZipRes = "androidRes.zip";
        public const string kIosZipFolder = "iosRes";
        public const string kIosZipRes = "iosRes.zip";
        public const string kWindowsZipFolder = "windowsRes";
        public const string kWindowsZipRes = "windowsRes.zip";
        public const string kPatchZipRes = "patch.zip";
        public const string kPatchDir = "patch";
        public const string kAssetBundle = "AssetBundle";
        public const string kPackCfg = "pack.cfg";
        public const string kBundleSuffix = ".unity3d";
        public const string kBundleSuffixNoPoint = "unity3d";
        public const string kPackCfgSuffix = ".cfg";
        public const string kPatchCfg = "patch.cfg";
        public const string kCodePatchFile = "Assembly-CSharp.patch.bytes";
        public const char kSlash = '/';
        public const string kVersionCfg = "version.asset";
    }
}
