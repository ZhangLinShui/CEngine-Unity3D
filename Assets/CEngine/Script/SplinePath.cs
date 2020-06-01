using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SplinePath
{
    [SerializeField]
    List<ControlPoint> _points;

    [SerializeField]
    bool _loop = false;

    public SplinePath(Vector2 position)
    {
        _points = new List<ControlPoint>
    {
      new ControlPoint(position, -Vector2.one, Vector2.one),
      new ControlPoint(position + Vector2.right, -Vector2.one, Vector2.one)
    };
    }

    public bool loop { get { return _loop; } set { _loop = value; } }

    public ControlPoint this[int i]
    {
        get { return _points[(_loop && i == _points.Count) ? 0 : i]; }
        set { _points[(_loop && i == _points.Count) ? 0 : i] = value; }
    }

    public int NumPoints { get { return _points.Count; } }

    public int NumSegments { get { return _points.Count - (_loop ? 0 : 1); } }

    public ControlPoint InsertPoint(int i, Vector2 position)
    {
        _points.Insert(i, new ControlPoint(position, -Vector2.one, Vector2.one));
        return this[i];
    }
    public ControlPoint RemovePoint(int i)
    {
        var item = this[i];
        _points.RemoveAt(i);
        return item;
    }
    public Vector2[] GetBezierPointsInSegment(int i)
    {
        var pointBack = this[i];
        var pointFront = this[i + 1];
        return new Vector2[4]
        {
      pointBack.position,
      pointBack.position + pointBack.tangentFront,
      pointFront.position + pointFront.tangentBack,
      pointFront.position
        };
    }

    public ControlPoint MovePoint(int i, Vector2 position)
    {
        this[i] = ControlPoint.MovePosition(this[i], position);
        return this[i];
    }

    public ControlPoint MoveTangentBack(int i, Vector2 position)
    {
        this[i] = ControlPoint.MoveTangentBack(this[i], position);
        return this[i];
    }

    public ControlPoint MoveTangentFront(int i, Vector2 position)
    {
        this[i] = ControlPoint.MoveTangentFront(this[i], position);
        return this[i];
    }
}