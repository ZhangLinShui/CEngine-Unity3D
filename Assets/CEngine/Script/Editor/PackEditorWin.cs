using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PackEditorWinCfg
{
    public int CurVersion;
    public int PatchVersion;
}

public class PackEditorWin : EditorWindow
{
    PackEditorWinCfg _cfg = new PackEditorWinCfg();

    private static string SaveKey = "packEditorWinKey";

    private void OnGUI()
    {
        GUILayout.Label("包版本");
        _cfg.CurVersion = int.Parse(GUILayout.TextField(_cfg.CurVersion.ToString()));
        GUILayout.Label("补丁版本");
        _cfg.PatchVersion = int.Parse(GUILayout.TextField(_cfg.PatchVersion.ToString()));
    }

    public static PackEditorWinCfg GetCfg()
    {
        var cfg = new PackEditorWinCfg();

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
