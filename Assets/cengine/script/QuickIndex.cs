using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickIndex : MonoBehaviour
{
    public string Key = "C_";

    public Dictionary<string, GameObject> _dict = new Dictionary<string, GameObject>();

#if UNITY_EDITOR
    public void Execute()
    {
        _dict.Clear();

        Index(transform);
    }

    private void Index(Transform tf)
    {
        foreach (Transform child in tf)
        {
            if (child.gameObject.name.StartsWith(Key))
            {
                if (_dict.ContainsKey(child.gameObject.name))
                {
                    Debug.LogError("repeated " + child.gameObject.name);
                }
                _dict[child.gameObject.name] = child.gameObject;
            }
            Index(child);
        }
    }
#endif

    public GameObject GetCObject(string name)
    {
        GameObject go;
        _dict.TryGetValue(name, out go);
        return go;
    }
}
