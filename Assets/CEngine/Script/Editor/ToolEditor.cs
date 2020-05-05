using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using CEngine;
using System.Text;
using System.Linq;

namespace CEngine
{
    public class ToolEditor
    {
        public static string DevCacheDirectory = "/DevCache/";
        public static string DiffPatchDirectory = "/DiffPatch/";

        public static string[] _suffixs = new string[] { ".prefab", ".png" };
        public static string kSpriteExtension = ".png";

        public static bool IsSuffixAssetBundle(string suf)
        {
            foreach (var s in _suffixs)
            {
                if (s.Equals(suf))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsSpriteExtension(string suf)
        {
            return suf == kSpriteExtension;
        }

        public static void OnLoadCreateDevDirectory(string relativePath)
        {
            var path = Application.dataPath + relativePath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        [InitializeOnLoadMethod]
        public static void OnInitialize()
        {
            OnLoadCreateDevDirectory(DevCacheDirectory);
            OnLoadCreateDevDirectory(DevCacheDirectory + AssetBundlePath.kWindows);
            OnLoadCreateDevDirectory(DevCacheDirectory + AssetBundlePath.kAndroid);
            OnLoadCreateDevDirectory(DevCacheDirectory + AssetBundlePath.kIos);

            OnLoadCreateDevDirectory(AssetBundlePath.kSlash + DiffPatchDirectory);
        }

        private static void DeleteDirectoryChild(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }

        [MenuItem("AssetBundleTool/修改版本配置", priority = 1)]
        public static void OpenPackEditor()
        {
            var win = EditorWindow.GetWindow<PackEditorWin>();
            win.Show();
        }

        public static void GeneratePackageCfg(string directoryPath)
        {
            var root = Path.GetFileNameWithoutExtension(directoryPath).ToLower();

            var packEditorCfg = PackEditorWin.GetCfg();

            var packCfg = new PackageCfg();
            packCfg.CurVersion = packEditorCfg.CurVersion;
            packCfg.PatchVersion = packEditorCfg.PatchVersion;
            packCfg.ForceUpdateVersion = packEditorCfg.ForceUpdateVersion;

            TraverseDirectory(directoryPath, packCfg, root);

            var dir = Directory.CreateDirectory(directoryPath);
            var filePath = dir.Parent.FullName + AssetBundlePath.kSlash + AssetBundlePath.kPackCfg;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (var sw = File.CreateText(filePath))
            {
                sw.Write(EditorJsonUtility.ToJson(packCfg));
            }
            var destFile = directoryPath + AssetBundlePath.kSlash + AssetBundlePath.kPackCfg;
            if (File.Exists(destFile))
            {
                File.Delete(destFile);
            }
            File.Copy(filePath, directoryPath + AssetBundlePath.kSlash + AssetBundlePath.kPackCfg);
        }

        public static void TraverseDirectory(string dirPath, PackageCfg pcfg, string root)
        {
            var files = Directory.GetFiles(dirPath);
            foreach (var file in files)
            {
                var correctFileName = file.Replace(@"\", @"/").ToLower();

                var ext = Path.GetExtension(correctFileName);
                var ret = AssetBundleMgr.instance.CompressExts.FirstOrDefault(c => c == ext);
                if (!string.IsNullOrEmpty(ret))
                {
                    pcfg.Files.Add(new FileCfg(CommonTool.CalFileMD5(correctFileName), GetRelativePath(correctFileName, root)));
                }
            }
            var dirs = Directory.GetDirectories(dirPath);
            foreach (var dir in dirs)
            {
                TraverseDirectory(dir, pcfg, root);
            }
        }

        public static string GetRelativePath(string path, string root)
        {
            var sb = new StringBuilder();
            var ps = path.Split(AssetBundlePath.kSlash);
            for(int i = ps.Length - 1; i >= 0; --i)
            {
                var p = ps[i];
                if (p == root)
                {
                    break;
                }
                if (i == ps.Length - 1)
                {
                    sb.Append(p);
                }
                else
                {
                    sb.Insert(0, p + AssetBundlePath.kSlash);
                }
            }
            return sb.ToString().ToLower();
        }

        [MenuItem("AssetBundleTool/拷贝压缩/Android", priority = 2)]
        public static void PackUncompressAndroidAB()
        {
            EditorUtility.DisplayProgressBar("", "", 0);
            var path = Application.dataPath + DevCacheDirectory + AssetBundlePath.kAndroid;
            GeneratePackageCfg(path);
            DeleteDirectoryChild(Application.streamingAssetsPath);
            ZipHelper.ZipDirectoryDirect(path, Application.streamingAssetsPath + AssetBundlePath.kSlash + AssetBundlePath.kZipRes);
            EditorUtility.ClearProgressBar();
            TimeLogger.LogYellow("压缩完成");
            AssetDatabase.Refresh();
        }

        [MenuItem("AssetBundleTool/拷贝压缩/Ios", priority = 2)]
        public static void PackUncompressIosAB()
        {
        }

        public static void Patch(PackageCfg cfg, string platform)
        {
            var tmpDir = Application.dataPath + "/tempDir";
            if (Directory.Exists(tmpDir))
            {
                Directory.Delete(tmpDir, true);
            }
            var patchCfg = new PackageCfg();
            var parentPath = Application.dataPath + DevCacheDirectory + platform;
            for (int i = 0; i < cfg.Files.Count; ++i)
            {
                var f = cfg.Files[i];
                var md5 = CommonTool.CalFileMD5(parentPath + AssetBundlePath.kSlash + f.Path);
                if (md5 != f.MD5 && Path.GetExtension(f.Path) != AssetBundlePath.kPackCfgSuffix)
                {
                    patchCfg.Files.Add(new FileCfg(md5, f.Path));

                    var t = tmpDir + AssetBundlePath.kSlash + f.Path;
                    if (!Directory.Exists(Path.GetDirectoryName(t)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(t));
                    }
                    File.Copy(parentPath + AssetBundlePath.kSlash + f.Path, t);
                }
            }
            if (patchCfg.Files.Count != 0)
            {
                using (var sw = File.CreateText(tmpDir + AssetBundlePath.kSlash + AssetBundlePath.kPatchCfg))
                {
                    sw.Write(EditorJsonUtility.ToJson(patchCfg));
                }
                ZipHelper.ZipDirectoryDirect(tmpDir, Application.dataPath + DiffPatchDirectory + platform + AssetBundlePath.kPatchZipRes);
                TimeLogger.LogYellow(platform +  "差异包生成成功");

                if (Directory.Exists(tmpDir))
                {
                    Directory.Delete(tmpDir, true);
                }
            }
        }

        [MenuItem("AssetBundleTool/生成差异包", priority = 3)]
        public static void GenerateDiffPatch()
        {
            var p = Application.dataPath + DevCacheDirectory + AssetBundlePath.kPackCfg;
            var jsonData = File.ReadAllText(p);
            var cfg = new PackageCfg();
            EditorJsonUtility.FromJsonOverwrite(jsonData, cfg);

            Patch(cfg, AssetBundlePath.kAndroid);
            //Patch(cfg, AssetBundlePath.kIos);
            Patch(cfg, AssetBundlePath.kWindows);

            AssetDatabase.Refresh();
        }

        [MenuItem("AssetBundleTool/打开持久化数据目录", priority = 4)]
        public static void OpenPersistentFolder()
        {
            var holderPath = Application.persistentDataPath + AssetBundlePath.kSlash + "holder";
            if (!File.Exists(holderPath))
            {
                var fs = File.Create(holderPath);
                fs.Dispose();
            }
            EditorUtility.RevealInFinder(holderPath);
        }

        [MenuItem("Assets/CreateAssetBundle")]
        public static void CreateAssetBundle()
        {
            foreach (var guid in Selection.assetGUIDs)
            {
                var p = AssetDatabase.GUIDToAssetPath(guid);

                if (AssetBundlePath.kAssetBundle == Path.GetFileName(p))
                {
                    Debug.LogError("directory name error " + AssetBundlePath.kAssetBundle);
                    return;
                }
                if (!Directory.Exists(p))
                {
                    Debug.LogError("not directory " + p);
                    return;
                }
                var isOk = false;
                var l = p.Split('/');
                StringBuilder sb = new StringBuilder();
                for (int i = l.Length - 1; i >= 0; --i)
                {
                    if (l[i] == AssetBundlePath.kAssetBundle)
                    {
                        isOk = true;
                        break;
                    }
                    sb.Insert(0, l[i] + (i != l.Length - 1 ? "/" : ""));
                }
                if (!isOk)
                {
                    Debug.LogError(string.Format("directory {0} not Assetbundle child directory", p));
                    return;
                }
                var abName = sb.ToString() + AssetBundlePath.kBundleSuffix;
                abName = abName.ToLower();

                var directory = Directory.CreateDirectory(Path.GetFullPath(p));
                if (0 != directory.GetDirectories().Length)
                {
                    Debug.LogError(string.Format("direcory {0} is not leaf node", p));
                }
                var files = directory.GetFiles();
                for (int i = 0; i < files.Length; ++i)
                {
                    var file = files[i];

                    if (IsSpriteExtension(Path.GetExtension(file.Name))) 
                    {
                        var ti = AssetImporter.GetAtPath(p + '/' + file.Name) as TextureImporter;
                        if (ti.textureType == TextureImporterType.Sprite)
                        {
                            ti.spritePackingTag = sb.ToString() + ".packTag";
                        }
                        ti.assetBundleName = abName;
                    }
                    else if (IsSuffixAssetBundle(Path.GetExtension(file.Name)))
                    {
                        var ti = AssetImporter.GetAtPath(p + '/' + file.Name);
                        ti.assetBundleName = abName;
                    }
                }
                var abEntry = new AssetBundleBuild();
                abEntry.assetBundleName = abName;
                abEntry.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(abName);

                var abArray = new AssetBundleBuild[1] { abEntry };

                BuildPipeline.BuildAssetBundles(Application.dataPath + DevCacheDirectory + AssetBundlePath.kWindows, abArray, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
                BuildPipeline.BuildAssetBundles(Application.dataPath + DevCacheDirectory + AssetBundlePath.kAndroid, abArray, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
                //BuildPipeline.BuildAssetBundles(Application.dataPath + DevCacheDirectory + AssetBundlePath.kIos, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.iOS);
                AssetDatabase.Refresh();
                TimeLogger.LogYellow("打包完成");
            }
        }
    }
}
