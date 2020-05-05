using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            GUILayout.Label("包版本");
            _cfg.CurVersion = int.Parse(GUILayout.TextField(_cfg.CurVersion.ToString()));
            GUILayout.Label("补丁版本[无补丁版本此值必须为0]");
            _cfg.PatchVersion = int.Parse(GUILayout.TextField(_cfg.PatchVersion.ToString()));
            GUILayout.Label("强更包版本");
            _cfg.ForceUpdateVersion = int.Parse(GUILayout.TextField(_cfg.ForceUpdateVersion.ToString()));
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
