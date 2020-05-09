using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using IFix.Core;
using GameLogic;

namespace CEngine
{
    public class UpdateMgr : SceneTemplate<UpdateMgr>
    {
        protected override void OnInit()
        {
            StartCoroutine(PackCoroutineEnter());
        }

        IEnumerator UncompressPackRes(bool isForce)
        {
            var zipFolderpath = Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundleMgr.instance.ZipFolder;
            if (isForce && Directory.Exists(zipFolderpath))
            {
                Directory.Delete(zipFolderpath, true);
            }
            var zipFilePath = Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundleMgr.instance.ZipRes;
            if (!Directory.Exists(zipFolderpath))
            {
                using (var req = UnityWebRequest.Get(AssetBundleMgr.instance.StreamingAssetPath + AssetBundleMgr.instance.ZipRes))
                {
                    yield return req.SendWebRequest();
                    if (req.isNetworkError || req.isHttpError)
                    {
                        Debug.LogError(req.error);
                        yield break;
                    }
                    if (File.Exists(zipFilePath))
                    {
                        File.Delete(zipFilePath);
                    }
                    File.WriteAllBytes(zipFilePath, req.downloadHandler.data);
                }
                var folderPath = Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundleMgr.instance.ZipFolder;
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                }
                Directory.CreateDirectory(folderPath);

                ZipHelper.UnZip(zipFilePath, folderPath);
            }
            if (File.Exists(zipFilePath))
            {
                File.Delete(zipFilePath);
            }
        }

        private void ForceUncompressLocalRes(string zipFilePath, string outputDir)
        {
            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
            Directory.CreateDirectory(outputDir);

            ZipHelper.UnZip(zipFilePath, outputDir);

            File.Delete(zipFilePath);
        }

        IEnumerator PackCoroutineEnter()
        {
            EventMgr.instance.SendEvent<float>(EventDefine.UpdateProgress, 0f);

            yield return StartCoroutine(UncompressPackRes(false));

            var mainPackCfg = new PackageCfg();
            var mainPackCfgPath = Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundleMgr.instance.ZipFolder + AssetBundlePath.kSlash + AssetBundlePath.kPackCfg;
            var jsonData = File.ReadAllText(mainPackCfgPath);
            JsonUtility.FromJsonOverwrite(jsonData, mainPackCfg);

            EventMgr.instance.SendEvent<float>(EventDefine.UpdateProgress, 0.05f);

            //覆盖安装检测(之前已经有了缓存文件)
            var InternalCfg = Resources.Load<VersionCfg>(Path.GetFileNameWithoutExtension(AssetBundlePath.kVersionCfg));
            if (mainPackCfg.CurVersion != InternalCfg.CurVersion)
            {
                yield return StartCoroutine(UncompressPackRes(true));
                jsonData = File.ReadAllText(mainPackCfgPath);
                JsonUtility.FromJsonOverwrite(jsonData, mainPackCfg);
            }

            EventMgr.instance.SendEvent<float>(EventDefine.UpdateProgress, 0.2f);

            //todo: 向服务器请求版本更新信息
            var updateUri = "file://" + Application.dataPath + "/Web/" + "UpdateJson";
            var patchUri = "file://" + Application.dataPath + "/DiffPatch/" + AssetBundleMgr.instance.PatchZip;
            using (var updateReq = UnityWebRequest.Get(updateUri))
            {
                EventMgr.instance.SendEvent<float>(EventDefine.UpdateProgress, 0.3f);

                yield return updateReq.SendWebRequest();
                if (updateReq.isHttpError || updateReq.isNetworkError)
                {
                    Debug.LogError(updateReq.error);
                    yield break;
                }
                EventMgr.instance.SendEvent<float>(EventDefine.UpdateProgress, 0.4f);

                var serverCfg = new PackageCfg();
                jsonData = System.Text.Encoding.UTF8.GetString(updateReq.downloadHandler.data);
                JsonUtility.FromJsonOverwrite(jsonData, serverCfg);
                if (mainPackCfg.CurVersion < serverCfg.ForceUpdateVersion)
                {
                    //todo:展示强更界面
                    yield break;
                }
                EventMgr.instance.SendEvent<float>(EventDefine.UpdateProgress, 0.5f);

                //处理补丁包
                var mainRoot = Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundleMgr.instance.ZipFolder + AssetBundlePath.kSlash;
                if (mainPackCfg.PatchVersion < serverCfg.PatchVersion)
                {
                    if (mainPackCfg.PatchVersion != 0)
                    {
                        yield return StartCoroutine(UncompressPackRes(true));
                    }
                    using (var patchReq = UnityWebRequest.Get(patchUri))
                    {
                        EventMgr.instance.SendEvent<float>(EventDefine.UpdateProgress, 0.6f);

                        yield return patchReq.SendWebRequest();
                        if (patchReq.isNetworkError || patchReq.isHttpError)
                        {
                            Debug.LogError(updateReq.error);
                            yield break;
                        }
                        EventMgr.instance.SendEvent<float>(EventDefine.UpdateProgress, 0.7f);

                        var patchZipPath = Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundleMgr.instance.PatchZip;
                        if (File.Exists(patchZipPath))
                        {
                            File.Delete(patchZipPath);
                        }
                        File.WriteAllBytes(patchZipPath, patchReq.downloadHandler.data);

                        ForceUncompressLocalRes(patchZipPath, Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundlePath.kPatchDir);
                    }

                    EventMgr.instance.SendEvent<float>(EventDefine.UpdateProgress, 0.8f);

                    //合并主包与补丁包配置
                    var mainPackDict = new Dictionary<string, FileCfg>();
                    foreach (var mf in mainPackCfg.Files)
                    {
                        mainPackDict[mf.Path] = mf;
                    }
                    var patchRoot = Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundlePath.kPatchDir + AssetBundlePath.kSlash;
                    var patchCfg = new PackageCfg();
                    var patchJsonData = File.ReadAllText(patchRoot + AssetBundlePath.kPatchCfg);
                    JsonUtility.FromJsonOverwrite(patchJsonData, patchCfg);
                    foreach (var f in patchCfg.Files)
                    {
                        var isPatchFile = Path.GetExtension(f.Path) == AssetBundleMgr.instance.kPatchFileExt;
                        if (!isPatchFile)
                        {
                            if (!mainPackDict.ContainsKey(f.Path))
                            {
                                Debug.LogError("patch file no mainPack file:" + f.Path);
                                yield break;
                            }
                            if (mainPackDict[f.Path].MD5 == f.MD5)
                            {
                                Debug.LogError("patchfile same as mainPack file:" + f.Path);
                                yield break;
                            }
                        }
                        if (File.Exists(mainRoot + f.Path))
                        {
                            File.Delete(mainRoot + f.Path);
                        }
                        File.Copy(patchRoot + f.Path, mainRoot + f.Path);
                        mainPackDict[f.Path] = f;
                    }
                    mainPackCfg.PatchVersion = patchCfg.PatchVersion;
                    mainPackCfg.Files = mainPackDict.Values.ToList();
                    var mergeCfgJsonData = JsonUtility.ToJson(mainPackCfg);
                    if (File.Exists(mainPackCfgPath))
                    {
                        File.Delete(mainPackCfgPath);
                    }
                    File.WriteAllText(mainPackCfgPath, mergeCfgJsonData);

                    Directory.Delete(patchRoot, true);
                }
                EventMgr.instance.SendEvent<float>(EventDefine.UpdateProgress, 0.9f);

                //进行文件完整性校验
                var md5 = "";
                foreach(var f in mainPackCfg.Files)
                {
                    if (!File.Exists(mainRoot + f.Path))
                    {
                        Debug.LogError("mainPack file not exist " + f.Path);
                        yield break;
                    }
                    if (Path.GetExtension(f.Path) == AssetBundlePath.kPackCfgSuffix)
                    {
                        continue;
                    }
                    md5 = CommonTool.CalFileMD5(mainRoot + f.Path);
                    if (md5 != f.MD5)
                    {
                        Debug.LogError(string.Format("mainPack file MD5 check error! cfg md5:{0} real md5:{1}", f.MD5, md5));
                        yield break;
                    }
                }

                //如果存在代码补丁就加载
                var codePatchFile = mainRoot + AssetBundlePath.kCodePatchFile;
                if (File.Exists(codePatchFile))
                {
                    PatchManager.Load(new MemoryStream(File.ReadAllBytes(codePatchFile)));
                }
                EventMgr.instance.SendEvent<float>(EventDefine.UpdateProgress, 1f);
            }
        }
    }
}
