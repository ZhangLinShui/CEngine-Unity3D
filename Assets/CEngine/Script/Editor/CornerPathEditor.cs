//********************************************************************
//	CopyRight   CEngine
//	Purpose:	角点路径编辑器类
//	Created:	2020-06-01
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CornerPath))]
public class CornerPathEditor : Editor
{
    private CornerPath _cp;
    private SerializedProperty _seqProp;

    private int selectId = 0;

    private void OnEnable()
    {
        _cp = (CornerPath)target;
        _seqProp = serializedObject.FindProperty("ShowSequence");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.HelpBox("total point : " + _cp.GetPath().Count + "\nselect point : " + selectId, MessageType.Info);
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(_seqProp);
        if (_cp.GetPath().Count > 0)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_path").GetArrayElementAtIndex(selectId));
        }
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    private void Input()
    {
        var curEvent = Event.current;

        var worldPoint = HandleUtility.GUIPointToWorldRay(curEvent.mousePosition).origin;
        var localPoint = _cp.transform.InverseTransformPoint(worldPoint);
        localPoint.z = 0f;

        if (curEvent.type == EventType.MouseDown && curEvent.button == 0 && curEvent.shift)
        {
            Undo.RecordObject(_cp, "add point");
            _cp.AddPoint(localPoint);
        }
        else if (curEvent.type == EventType.MouseDown && curEvent.button == 0 && curEvent.control)
        {
            var path = _cp.GetPath();
            if (path.Count > 0)
            {
                for (int i = path.Count - 1; i >= 0; --i)
                {
                    if (Vector3.Distance(path[i], localPoint) < 0.1f)
                    {
                        Undo.RecordObject(_cp, "remove point");
                        path.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

    private void Draw()
    {
        var path = _cp.GetPath();
        for(int i = 0; i < path.Count; ++i)
        {
            var p = path[i];
            Handles.color = i == selectId ? Handles.selectedColor : Color.red;
            var id = EditorGUIUtility.GetControlID(FocusType.Passive);
            var oldp = _cp.transform.TransformPoint(p);
            var size = HandleUtility.GetHandleSize(oldp) * .05f;
            var np = Handles.FreeMoveHandle(id, oldp, Quaternion.identity, size, Vector3.zero, Handles.DotHandleCap);
            if (id == EditorGUIUtility.hotControl)
            {
                selectId = i;
            }
            if (oldp != np)
            {
                Undo.RecordObject(_cp, "move point");
                _cp.SetPoint(i, np);
            }
            if (_cp.ShowSequence)
            {
                Handles.Label(np, i.ToString());
            }
        }
        Repaint();
    }

    private void OnDisable()
    {
    }

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    static void DrawGizmo(CornerPath cp, GizmoType gt)
    {
        Gizmos.color = Color.green;
        var segs = cp.GetPath();
        if (segs.Count >= 2)
        {
            for (int i = 0; i < segs.Count - 1; ++i)
            {
                Gizmos.DrawLine(cp.transform.TransformPoint(segs[i]), cp.transform.TransformPoint(segs[i + 1]));
            }
        }
    }
}
