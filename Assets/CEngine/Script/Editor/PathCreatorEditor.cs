using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathCreatorEditor : Editor
{
    PathCreator creator;
    SplinePath path;
    SerializedProperty property;
    static int selectedControlPointId = -1;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var loopProp = property.FindPropertyRelative("_loop");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(loopProp);
        var ptsProp = property.FindPropertyRelative("_points");
        var msg = "Total points in path: " + ptsProp.arraySize + "\n";
        if (selectedControlPointId >= 0 && ptsProp.arraySize > 0)
        {
            EditorGUILayout.HelpBox(msg + "Selected control point: " + selectedControlPointId, MessageType.Info);
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(ptsProp.GetArrayElementAtIndex(selectedControlPointId), true);
        }
        else
        {
            EditorGUILayout.HelpBox(msg + "No control points selected", MessageType.Info);
        }
        if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        mousePos = creator.transform.InverseTransformPoint(mousePos);
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Undo.RecordObject(creator, "Insert point");
            path.InsertPoint(path.NumPoints, mousePos);
        }
        else if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.control)
        {
            for (int i = 0; i < path.NumPoints; i++)
            {
                if (Vector2.Distance(mousePos, path[i].position) <= .25f)
                {
                    Undo.RecordObject(creator, "Remove point");
                    path.RemovePoint(i);
                    break;
                }
            }
        }
    }

    void Draw()
    {
        Handles.matrix = creator.transform.localToWorldMatrix;
        var rot = Quaternion.Inverse(creator.transform.rotation) * Tools.handleRotation;
        var snap = Vector2.zero;
        Handles.CapFunction cap = Handles.DotHandleCap;
        for (int i = 0; i < path.NumPoints; i++)
        {
            float size;
            var pos = path[i].position;
            size = HandleUtility.GetHandleSize(pos) * .05f;
            Handles.Label(pos, i.ToString());
            Handles.color = i == selectedControlPointId ? Handles.selectedColor : Color.red;
            int ctrlId = GUIUtility.GetControlID(FocusType.Passive);
            Vector2 newPos = Handles.FreeMoveHandle(ctrlId, pos, rot, size, snap, cap);
            if (ctrlId == EditorGUIUtility.hotControl) selectedControlPointId = i;
            if (pos != newPos)
            {
                Undo.RecordObject(creator, "Move point position");
                path.MovePoint(i, newPos);
            }
            pos = newPos;
            Handles.color = Color.black;
            if (path.loop || i != 0)
            {
                var tanBack = pos + path[i].tangentBack;
                Handles.DrawLine(pos, tanBack);
                size = HandleUtility.GetHandleSize(tanBack) * .03f;
                Vector2 newTanBack = Handles.FreeMoveHandle(tanBack, rot, size, snap, cap);
                if (tanBack != newTanBack)
                {
                    Undo.RecordObject(creator, "Move point tangent");
                    path.MoveTangentBack(i, newTanBack - pos);
                }
            }
            if (path.loop || i != path.NumPoints - 1)
            {
                var tanFront = pos + path[i].tangentFront;
                Handles.DrawLine(pos, tanFront);
                size = HandleUtility.GetHandleSize(tanFront) * .03f;
                Vector2 newTanFront = Handles.FreeMoveHandle(tanFront, rot, size, snap, cap);
                if (tanFront != newTanFront)
                {
                    Undo.RecordObject(creator, "Move point tangent");
                    path.MoveTangentFront(i, newTanFront - pos);
                }
            }
        }
        Repaint();
    }



    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawGizmo(PathCreator creator, GizmoType gizmoType)
    {
        Gizmos.matrix = creator.transform.localToWorldMatrix;
        var path = creator.path;
        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector2[] points = path.GetBezierPointsInSegment(i);
            var pts = Handles.MakeBezierPoints(points[0], points[3], points[1], points[2], 30);
            Gizmos.color = Color.green;
            for (int j = 0; j < pts.Length - 1; j++)
            {
                Gizmos.DrawLine(pts[j], pts[j + 1]);
            }
        }
    }

    void OnEnable()
    {
        creator = (PathCreator)target;
        path = creator.path ?? creator.CreatePath();
        property = serializedObject.FindProperty("path");
    }
}