using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

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
            var dataPath = Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundleMgr.instance.ZipRes;
            if (isForce && Directory.Exists(dataPath))
            {
                Directory.Delete(dataPath, true);
            }
            if (!Directory.Exists(dataPath))
            {
                var zipFilePath = Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundleMgr.instance.ZipRes;
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
        }

        private void ForceUncompressLocalRes(string zipFilePath, string outputDir)
        {
            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
            Directory.CreateDirectory(outputDir);

            ZipHelper.UnZip(zipFilePath, outputDir);
        }

        IEnumerator PackCoroutineEnter()
        {
            yield return StartCoroutine(UncompressPackRes(false));

            var mainPackCfg = new PackageCfg();
            var mainPackCfgPath = Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundleMgr.instance.ZipFolder + AssetBundlePath.kSlash + AssetBundlePath.kPackCfg;
            var jsonData = File.ReadAllText(mainPackCfgPath);
            JsonUtility.FromJsonOverwrite(jsonData, mainPackCfg);

            //todo: 向服务器请求版本更新信息
            var updateUri = "file://" + Application.dataPath + "/Web/" + "UpdateJson";
            var patchUri = "file://" + Application.dataPath + "/DiffPatch/" + AssetBundleMgr.instance.PatchZip;
            using (var updateReq = UnityWebRequest.Get(updateUri))
            {
                yield return updateReq.SendWebRequest();
                if (updateReq.isHttpError || updateReq.isNetworkError)
                {
                    Debug.LogError(updateReq.error);
                    yield break;
                }
                var serverCfg = new PackageCfg();
                jsonData = System.Text.Encoding.UTF8.GetString(updateReq.downloadHandler.data);
                JsonUtility.FromJsonOverwrite(jsonData, serverCfg);
                if (mainPackCfg.CurVersion < serverCfg.ForceUpdateVersion)
                {
                    //todo:展示强更界面
                    yield break;
                }
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
                        yield return patchReq.SendWebRequest();
                        if (patchReq.isNetworkError || patchReq.isHttpError)
                        {
                            Debug.LogError(updateReq.error);
                            yield break;
                        }
                        var patchZipPath = Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundleMgr.instance.PatchZip;
                        if (File.Exists(patchZipPath))
                        {
                            File.Delete(patchZipPath);
                        }
                        File.WriteAllBytes(patchZipPath, patchReq.downloadHandler.data);

                        ForceUncompressLocalRes(patchZipPath, Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundlePath.kPatchDir);
                    }
                    //合并主包与补丁包配置
                    var mainPackDict = new Dictionary<string, FileCfg>();
                    foreach (var mf in mainPackCfg.Files)
                    {
                        mainPackDict[mf.Path] = mf;
                    }
                    var patchRoot = Application.persistentDataPath + AssetBundlePath.kSlash + AssetBundlePath.kPatchDir + AssetBundlePath.kSlash;
                    var patchCfg = new PackageCfg();
                    var patchJsonData = File.ReadAllText(patchRoot + AssetBundlePath.kPackCfg);
                    JsonUtility.FromJsonOverwrite(patchJsonData, patchCfg);
                    foreach (var f in patchCfg.Files)
                    {
                        if (mainPackDict[f.Path].MD5 == f.MD5)
                        {
                            Debug.LogError("patchfile same as mainPack file:" + f.Path);
                            continue;
                        }
                        if (mainPackDict.ContainsKey(f.Path) || Path.GetExtension(f.Path) == AssetBundleMgr.instance.kPatchFileExt)
                        {
                            File.Copy(patchRoot + f.Path, mainRoot + f.Path);
                            mainPackDict[f.Path].MD5 = f.MD5;
                        }
                        else
                        {
                            Debug.LogError("patch file no mainPack file:" + f.Path);
                        }
                    }
                    mainPackCfg.PatchVersion = patchCfg.PatchVersion;
                    mainPackCfg.Files = mainPackDict.Values.ToList();
                    var mergeCfgJsonData = JsonUtility.ToJson(mainPackCfg);
                    if (File.Exists(mainPackCfgPath))
                    {
                        File.Delete(mainPackCfgPath);
                    }
                    File.WriteAllText(mainPackCfgPath, mergeCfgJsonData);
                }
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
                //加载完成 进入场景
            }
        }
    }
}
