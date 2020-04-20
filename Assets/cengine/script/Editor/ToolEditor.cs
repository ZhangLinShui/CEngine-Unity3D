using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using CEngine;
using System.Text;

public class ToolEditor
{
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
            var directory = Directory.CreateDirectory(Path.GetFullPath(p));
            if (null != directory.GetDirectories())
            {
                Debug.LogError(string.Format("direcory {0} is not leaf node", p));
            }

        }
    }
}
