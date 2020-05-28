//********************************************************************
//	CopyRight   CEngine
//	Purpose:	AB打包编辑器窗口类
//	Created:	2020-04-28
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace CEngine
{
    public class PackEditorWin : EditorWindow
    {
        PackageCfg _cfg = new PackageCfg();

        private static string SaveKey = "packEditorWinKey";

        private void OnGUI()
        {
            GUILayout.Label("当前包版本[出包时修改]");
            _cfg.CurVersion = int.Parse(Regex.Replace(GUILayout.TextField(_cfg.CurVersion.ToString()), "[^0-9]", ""));
            GUILayout.Label("强更包版本");
            _cfg.ForceUpdateVersion = int.Parse(Regex.Replace(GUILayout.TextField(_cfg.ForceUpdateVersion.ToString()), "[^0-9]", ""));
            GUILayout.Label("补丁版本[出包时必须为0][出补丁时才可修改]");
            _cfg.PatchVersion = int.Parse(Regex.Replace(GUILayout.TextField(_cfg.PatchVersion.ToString()), "[^0-9]", ""));
        }

        public static PackageCfg GetCfg()
        {
            var cfg = new PackageCfg();

            var d = EditorPrefs.GetString(SaveKey, "");
            if (!string.IsNullOrEmpty(d))
            {
                EditorJsonUtility.FromJsonOverwrite(d, cfg);
            }
            return cfg;
        }

        private void OnEnable()
        {
            var d = EditorPrefs.GetString(SaveKey, "");
            if (!string.IsNullOrEmpty(d))
            {
                EditorJsonUtility.FromJsonOverwrite(d, _cfg);
            }
        }

        private void OnDisable()
        {
            EditorPrefs.SetString(SaveKey, EditorJsonUtility.ToJson(_cfg));
        }
    }
}
