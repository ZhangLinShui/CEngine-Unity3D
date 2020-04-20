using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;
using UnityEditor;

[CustomEditor(typeof(QuickIndex))]
public class QuickIndexEditor : Editor
{
    private QuickIndex _tool;

    private void OnEnable()
    {
        _tool = (QuickIndex)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Execute"))
        {
            _tool.Execute();
        }
    }
}
